# CTA Ridership Analysis
N-tier C# and SQL program to analyze CTA Ridership data for Programming Language Design &amp; Implementation (CS341)

Upon user selection of different stations and stops, the application analyzes and displays information such as total ridership for the selected station, average ridership, ridership on the weekday, direction of travel for a particular stop, and lines available for a particular stop. There are also top-10 options that display the top-10 stations for a criterion with features described above. Additionally, the users can update the handicap accessible field assuming in a production use, the application is connected to a database used by everyone. Below is a gif displaying basically what the app does. 

![CTA Ridership App](/cta.gif)

## Download
You can download the compiled executable [here](https://drive.google.com/drive/folders/0BzjJ5VL34cNvbkd0Q2lJeXhHQnc?usp=sharing). **Note: works only on Windows operating system.**

### Installation & uninstalling
1. Download the **CTA** folder as a whole.
2. Unzip the **CTA** folder into a directory of your choice.
3. Open the folder and run **CTA.application** or just **CTA**. 
4. A separate window will pop up. Click install to install program.
4. The program will pop up. Otherwise, click on **CTA.application**.

The program is listed as **CTA**. To uninstall:
1. Navigate to the uninstall window; Control Panel -> Uninstall or change a program.
2. Find and click on **CTA**.
3. Click on Uninstall.

### App usage
1. Load file by going to **File -> Load Stations**.
2. Click on a station to populate the ridership data and stops at the station.
3. Click on a stop to populate the rest of the information.

#### App usage issues
If given an error when attempting to load stations that looks like this:  
> *Error in Business.GetStations: An attempt to attach an auto-named database for file ... failed. ...*  

Replace "**|DataDirectory|**" with the full path to the **CTA.mdf** file. For example, if the file is located on the desktop, the full string would be "**C:\Users\\_%name of user%_\Desktop\CTA.mdf**".
