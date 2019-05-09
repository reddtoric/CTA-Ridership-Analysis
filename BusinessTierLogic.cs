//
// BusinessTier:  business logic, acting as interface between UI and data store.
//

using System;
using System.Collections.Generic;
using System.Data;


namespace BusinessTier
{

    //
    // Business:
    //
    public class Business
    {
        //
        // Fields:
        //
        private string _DBFile;
        private DataAccessTier.Data dataTier;


        ///
        /// <summary>
        /// Constructs a new instance of the business tier.  The format
        /// of the filename should be either |DataDirectory|\filename.mdf,
        /// or a complete Windows pathname.
        /// </summary>
        /// <param name="DatabaseFilename">Name of database file</param>
        /// 
        public Business(string DatabaseFilename)
        {
            _DBFile = DatabaseFilename;

            dataTier = new DataAccessTier.Data(DatabaseFilename);
        }


        ///
        /// <summary>
        ///  Opens and closes a connection to the database, e.g. to
        ///  startup the server and make sure all is well.
        /// </summary>
        /// <returns>true if successful, false if not</returns>
        /// 
        public bool TestConnection()
        {
            return dataTier.OpenCloseConnection();
        }


        ///
        /// <summary>
        /// Returns all the CTA Stations, ordered by name.
        /// </summary>
        /// <returns>Read-only list of CTAStation objects</returns>
        /// 
        public IReadOnlyList<CTAStation> GetStations()
        {
            List<CTAStation> stations = new List<CTAStation>();

            try
            {
                string sql = String.Format(@"
select * from Stations
order by Name
");
                //for every row in the table returned, create CTAStation object and insert end of the list of CTA Station
                DataSet ds = dataTier.ExecuteNonScalarQuery(sql);
                foreach (DataRow row in ds.Tables["TABLE"].Rows)
                {
                    CTAStation oneStation = new CTAStation(Convert.ToInt32(row["StationID"]), row["Name"].ToString());
                    stations.Add(oneStation);
                }

            }
            catch (Exception ex)
            {
                string msg = string.Format("Error in Business.GetStations: '{0}'", ex.Message);
                throw new ApplicationException(msg);
            }

            return stations;
        }


        ///
        /// <summary>
        /// Returns the CTA Stops associated with a given station,
        /// ordered by name.
        /// </summary>
        /// <returns>Read-only list of CTAStop objects</returns>
        ///
        public IReadOnlyList<CTAStop> GetStops(int stationID)
        {
            List<CTAStop> stops = new List<CTAStop>();

            try
            {
                string sql = String.Format(@"
select * from Stops
where Stops.StationID = {0}
order by Stops.Name
", stationID);

                //for every row in the table returned, create CTAStop object and insert end of the list of CTA Stop
                DataSet ds = dataTier.ExecuteNonScalarQuery(sql);
                foreach (DataRow row in ds.Tables["TABLE"].Rows)
                {
                    int stopID = Convert.ToInt32(row["StopID"]);
                    string stopName = row["Name"].ToString();
                    string direction = row["Direction"].ToString();
                    bool ada = Convert.ToBoolean(row["ADA"]);
                    double latitude = Convert.ToDouble(row["Latitude"]);
                    double longitude = Convert.ToDouble(row["Longitude"]);

                    CTAStop oneStop = new CTAStop(stopID, stopName, stationID, direction, ada, latitude, longitude);
                    stops.Add(oneStop);
                }

            }
            catch (Exception ex)
            {
                string msg = string.Format("Error in Business.GetStops: '{0}'", ex.Message);
                throw new ApplicationException(msg);
            }

            return stops;
        }


        ///
        /// <summary>
        /// Returns the top N CTA Stations by ridership, 
        /// ordered by name.
        /// </summary>
        /// <returns>Read-only list of CTAStation objects</returns>
        /// 
        public IReadOnlyList<CTAStation> GetTopStations(int N)
        {
            if (N < 1)
                throw new ArgumentException("GetTopStations: N must be positive");

            List<CTAStation> stations = new List<CTAStation>();

            try
            {
                string sql = String.Format(@"
select Stations.StationID, Name from Stations
inner join (
    select top {0} sum (Riderships.DailyTotal) as TotalRidership, Riderships.StationID from Riderships
    group by Riderships.StationID
    order by sum (Riderships.DailyTotal) desc
    ) as T
on Stations.StationID = T.StationID
order by Name
", N);

                //for every row in the table returned, create CTAStation object and insert end of the list of CTA Station
                DataSet ds = dataTier.ExecuteNonScalarQuery(sql);
                foreach (DataRow row in ds.Tables["TABLE"].Rows)
                {
                    CTAStation oneStation = new CTAStation(Convert.ToInt32(row["StationID"]), row["Name"].ToString());
                    stations.Add(oneStation);
                }

            }
            catch (Exception ex)
            {
                string msg = string.Format("Error in Business.GetTopStations: '{0}'", ex.Message);
                throw new ApplicationException(msg);
            }

            return stations;
        }

        // returns int stationID given stationName
        public int GetStationID(string stationName)
        {
            stationName = stationName.Replace("'", "''");
            string sql = String.Format(@"
select StationID from Stations
where Name = '{0}'
", stationName);

            return Convert.ToInt32(dataTier.ExecuteScalarQuery(sql));
        }

        // returns int total riderships given stationID
        public int GetTotalRidership(int stationID)
        {
            string sql = String.Format(@"
select sum(DailyTotal) from Riderships
where StationID = {0}
", stationID);

            return Convert.ToInt32(dataTier.ExecuteScalarQuery(sql));
        }

        // returns double avg riderships given stationID
        public double GetAvgRidership(int stationID)
        {
            string sql = String.Format(@"
select avg(DailyTotal) from Riderships
where StationID = {0}
", stationID);

            return Convert.ToDouble(dataTier.ExecuteScalarQuery(sql));
        }

        // returns double % riderships given stationID
        public double GetPercentageRidership(int stationID)
        {
            int totalRidership = this.GetTotalRidership(stationID);
            string sql = String.Format(@"
select sum(convert(BigInt, DailyTotal)) from Riderships
");

            object totalOverall = dataTier.ExecuteScalarQuery(sql);
            return Convert.ToDouble(totalRidership) / Convert.ToDouble(totalOverall) * 100.0;
        }

        // returns int Riderships for Weekday given stationID
        public int GetRidershipsWeekday(int stationID)
        {
            string sql = String.Format(@"
select sum(DailyTotal) from Riderships
where StationID = {0} and TypeOfDay = 'W'
", stationID);

            return Convert.ToInt32(dataTier.ExecuteScalarQuery(sql));
        }

        // returns int Riderships for Saturday given stationID
        public int GetRidershipsSaturday(int stationID)
        {
            string sql = String.Format(@"
select sum(DailyTotal) from Riderships
where StationID = {0} and TypeOfDay = 'A'
", stationID);

            return Convert.ToInt32(dataTier.ExecuteScalarQuery(sql));
        }

        //returns int Riderships for Sun/Holiday given stationID
        public int GetRidershipsSundayHoliday(int stationID)
        {
            string sql = String.Format(@"
select sum(DailyTotal) from Riderships
where StationID = {0} and TypeOfDay = 'U'
", stationID);

            return Convert.ToInt32(dataTier.ExecuteScalarQuery(sql));
        }

        //returns int stopID given stopName
        public int GetStopID(string stopName)
        {
            stopName = stopName.Replace("'", "''");
            string sql = String.Format(@"
select StopID from Stops
where Name = '{0}'
", stopName);

            return Convert.ToInt32(dataTier.ExecuteScalarQuery(sql));
        }

        //returns boolean ADA given stopID and stationID
        public bool GetADA(int stopID, int stationID)
        {
            string sql = string.Format(@"
SELECT ADA FROM Stops
WHERE StopID = '{0}' AND StationID = {1};
", stopID, stationID);

            return Convert.ToBoolean(dataTier.ExecuteScalarQuery(sql));
        }

        //returns boolean ADA given stop name and station name
        public bool GetADA(string stopName, string stationName)
        {
            return GetADA(GetStopID(stopName), GetStationID(stationName));
        }

        //returns string direction given stopID and stationID
        public string GetDirection(int stopID, int stationID)
        {
            string sql = string.Format(@"
SELECT Direction FROM Stops
WHERE StopID = '{0}' AND StationID = {1};
", stopID, stationID);

            return Convert.ToString(dataTier.ExecuteScalarQuery(sql));
        }

        //returns double latitude given stopID and stationID
        public double GetLatitude(int stopID, int stationID)
        {
            string sql = string.Format(@"
SELECT Latitude FROM Stops
WHERE StopID = '{0}' AND StationID = {1};
", stopID, stationID);

            return Convert.ToDouble(dataTier.ExecuteScalarQuery(sql));
        }

        //returns double longitude given stopID and stationID        
        public double GetLongitude(int stopID, int stationID)
        {
            string sql = string.Format(@"
SELECT Longitude FROM Stops
WHERE StopID = '{0}' AND StationID = {1};
", stopID, stationID);

            return Convert.ToDouble(dataTier.ExecuteScalarQuery(sql));
        }

        //returns read-only list of string of lines at a stop given stopID
        public IReadOnlyList<string> GetLinesAtStop(int stopID)
        {
            List<String> lines = new List<String>();

            try
            {
                string sql = String.Format(@"
SELECT Color
FROM Lines
INNER JOIN StopDetails ON Lines.LineID = StopDetails.LineID
INNER JOIN Stops ON StopDetails.StopID = Stops.StopID
WHERE Stops.StopID = {0}
ORDER BY Color ASC;
", stopID);

                DataSet ds = dataTier.ExecuteNonScalarQuery(sql);
                foreach (DataRow row in ds.Tables["TABLE"].Rows)
                {
                    string color = row["Color"].ToString();
                    lines.Add(color);
                }

            }
            catch (Exception ex)
            {
                string msg = string.Format("Error in Business.GetTopStations: '{0}'", ex.Message);
                throw new ApplicationException(msg);
            }

            return lines;
        }

        //returns read-only list of CTAStation objects given string partial station name
        public IReadOnlyList<CTAStation> FindStations(string partialStationName)
        {
            List<CTAStation> stations = new List<CTAStation>();

            try
            {
                partialStationName = partialStationName.Replace("'", "''");
                string sql = String.Format(@"
select StationID, Name from Stations
where Name like '%{0}%'
", partialStationName);

                DataSet ds = dataTier.ExecuteNonScalarQuery(sql);
                foreach (DataRow row in ds.Tables["TABLE"].Rows)
                {
                    CTAStation oneStation = new CTAStation(Convert.ToInt32(row["StationID"]), row["Name"].ToString());
                    stations.Add(oneStation);
                }

            }
            catch (Exception ex)
            {
                string msg = string.Format("Error in Business.GetTopStations: '{0}'", ex.Message);
                throw new ApplicationException(msg);
            }

            return stations;
        }

        //private; returns read-only list of CTAStation objects of top N stations by riderships given a type of Day
        private IReadOnlyList<CTAStation> GetTopStationsTypeOfDay(int N, string typeOfDay)
        {
            if (N < 1)
                throw new ArgumentException("GetTopStations: N must be positive");

            List<CTAStation> stations = new List<CTAStation>();

            try
            {
                string sql = String.Format(@"
select Stations.StationID, Name from Stations
inner join (
    select top {0} sum (Riderships.DailyTotal) as TotalRidership, Riderships.StationID from Riderships
    where TypeOfDay = '{1}'
    group by Riderships.StationID
    order by sum (Riderships.DailyTotal) desc
    ) as T
on Stations.StationID = T.StationID
order by Name
", N, typeOfDay);

                DataSet ds = dataTier.ExecuteNonScalarQuery(sql);
                foreach (DataRow row in ds.Tables["TABLE"].Rows)
                {
                    CTAStation oneStation = new CTAStation(Convert.ToInt32(row["StationID"]), row["Name"].ToString());
                    stations.Add(oneStation);
                }

            }
            catch (Exception ex)
            {
                string msg = string.Format("Error in Business.GetTopStations: '{0}'", ex.Message);
                throw new ApplicationException(msg);
            }

            return stations;
        }

        //returns read-only list of CTAStation objects of top N stations by weekday riderships
        public IReadOnlyList<CTAStation> GetTopStationsWeekDay(int N)
        {
            return this.GetTopStationsTypeOfDay(N, "W");
        }

        //returns read-only list of CTAStation objects of top N stations by saturday riderships
        public IReadOnlyList<CTAStation> GetTopStationsSaturday(int N)
        {
            return this.GetTopStationsTypeOfDay(N, "A");
        }

        //returns read-only list of CTAStation objects of top N stations by sunday/holiday riderships
        public IReadOnlyList<CTAStation> GetTopStationsSundayHoliday(int N)
        {
            return this.GetTopStationsTypeOfDay(N, "U");
        }

        //updates ADA field of a particular station and stop in the Stops table
        public void UpdateADA(bool revisedADA, string stationName, string stopName)
        {
            try
            {
                int intRevisedADA = revisedADA ? 1 : 0;
                int stationID = this.GetStationID(stationName);
                int stopID = this.GetStopID(stopName);
                string sql = String.Format(@"
update Stops
set ADA = {0}
where StationID = {1} and StopID = {2}
", intRevisedADA, stationID, stopID);

                int numRowsUpdated = dataTier.ExecuteActionQuery(sql);
            }
            catch (Exception ex)
            {
                string msg = string.Format("Error in Business.GetTopStations: '{0}'", ex.Message);
                throw new ApplicationException(msg);
            }
        }
    }//class
}//namespace
