
## Description
This app is a very simple application tracking fake students enrolled in a small set of schools.

## Instructions
1. Download the zip file of this code. 
2. Unzip locally and open in Visual Studio. This should work in both Windows and Mac versions of Visual Studio 2022.
3. Email christian.whiting@ccsheriff.org to receive the database connection string. The information in the connection string should also allow you to connect to a database explorer (the Server Explorer tool in Visual Studio or the Azure Data Studio application would work well for exploring the dataset).
4. Complete the tasks outlined below. 
5. Once completed, push your code to your own public repository and send the link to christian.whiting@ccsheriff.org and charles.martinez@ccsheriff.org.



## Tasks 
If you have any questions about the following tasks, please email christian.whiting@ccsheriff.org.

1. **Add the School column to the Student table** - Display which school each student is attending in the front end student table. 
2. **Complete the Add Student modal form** - Generate the school and student organization dropdown lists from the the database. Add server side code to save the submitted model to the database. You will need to convert the **SelectedOrgs** property of  **Students** to **OrgAssignments** and save to the database. Don't worry about validating the inputs.  
3. **Complete the link in the Students column of the Schools table** - The link should navigate to a page displaying the students that go to the school correlating to the row clicked. A new controller action and view will need to be created to accomplish this. 
