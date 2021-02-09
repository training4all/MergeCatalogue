# Catalogues
Console app that will merge catalogues from 2 companies based on certain business rules.

# Business Requirement:
- Company A have acquired Company B, A and B sell some common products, sourced from suppliers (Sometimes the same supplier, sometimes a different one).
- The business wants to consolidate the product catalog into one superset (merged catalog).

For more detailed rules, you can refer to ReadMe at https://github.com/tosumitagrawal/codingskills

# Installation
- I have build the solution as Console App using .Net Core 3.1 Framework.
- IDE used is Visual Studio 2019 and OS is Windows 10 Pro.
- To test the app locally, simple clone repo from **https://github.com/training4all/Catalogues.git**
- Open solution either using VS or VS Code, whatever you prefer.
- Build the solution and once successfully build, run the app.
- If you can see the console window asking for '**Enter Input folder path**' that means you have successfully cloned and setup the project.
- Final **'result_output.csv '** will be created inside output folder, whose path is configurable under **'Constants.cs'**, by default it's set to 
 ```sh
     public const string OutputFolder = @"C:\Output";
 ```
 you can either change the location in file or if system is not able to access the specified location then a default output folder will be created inside the executable location.
 For instance, my solution executable is at location 
  ```sh
     $ C:\projects\Catalogue\bin\Debug\netcoreapp3.1
     Hence, my default output folder will be created at location
     $ C:\projects\Catalogue\bin\Debug\netcoreapp3.1\output
 ```
 
# How to use the app
## Assumptions
- Below are the list of csv files that needs to be in the input folder for solution to work and produce result csv.
  - catalogA.csv - Products for Company A
  - catalogB.csv - Products for Company B
  - suppliersA.csv - List of Suppliers for Company A
  - suppliersB.csv - List of Suppliers for Company B
  - barcodesA.csv - Product barcodes provided by supplier for company A
  - barcodesB.csv - Product barcodes provided by supplier for company B
  
## Tech Stack
- **[.Net Core 3.1]** - Framework.
- **[C#]** - Language.
- **[XUnit, Moq]** - Unit Testing.
- **[IDE]** - VS 2019
- **[NLogger]** - Logging.
- **[DI]** - To inject dependencies at run time.
- **[CsvHelper]** - .NET library for working with csv files.
- **[Resharper]** - Code quality.

## Solution
- By comparing the csv files shared inside **'input'** and **'output'** folder inside git https://github.com/tosumitagrawal/codingskills.
- I observed that company A has preference over company B. For instance, consider product **Walkers Special Old Whiskey** which is has barcode same for both companies A and B
But in result csv we have that product mentioned using **SKU** and **Description** from company A.

Company A
| SKU | Description |
| ------ | ------ |
| 647-vyk-317 | Walkers Special Old Whiskey |

Company B
| SKU | Description |
| ------ | ------ |
| 999-vyk-317 | Walkers Special Old Whiskey test |

Result 
| SKU | Description | Source
| ------ | ------ | ------ |
| 647-vyk-317 | Walkers Special Old Whiskey | A |
- Since, I know that A has preference over B, thus I moved all products of company A to result csv.
- Then, I extract **[Barcodes B - Barcodes A]** barcodes from company B which are not there in company A, that means those products are unique to company B and hence,
needs to be present in result csv.

## How to test the app on local system
- Once you have done all steps listed under section **Installation**
- Simple run the app and that will open the console window.
- Console window will ask you to enter the **input folder path** which basically contains all the csv files for solution to refer.
You can have the csv files at whatever location you prefer, but, to ease the process I have created the input folder inside the solution for you to refer.
Location of input folder shipped with this solution is
 ```sh
    $ <your_solution_folder>/input
    In my case, I have solution checked out at location, c:/projects/Catalogue,
    hence my input folder location is
    $ c:/projects/Catalogue/input
 ```
 - After inputting the input folder, simple press enter and it will start processing.
 - Few logs will be generated in console window. All information will be displayed as **[INFO]** and all errors will be listed as **[Error]**
 - At the end of processing, you can notice that there is a informational log, printting the location of output folder where the output csv 
 file called **'result_output.csv'** will be generated.
 - Simple navigate to that folder and open **'result_output.csv'** to verify the output.
 - Below is the snapshot with highlighted areas to give an idea of what a console window will look like after processing.
![catalogue](https://user-images.githubusercontent.com/17959609/107311518-9af78d00-6ae2-11eb-892a-d4606680b53e.png)

**Enjoy, Catalogue App !!!**
