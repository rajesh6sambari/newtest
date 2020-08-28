Assumptions: custom attributes will contain "Attribute"

Zipped folders have <folder name> -> <project folder> structure when unzipped

Tests contained under same namespace

----------------------------------------------------------------------------------

Issues:

When deleting workspace after sending feedback dto access to the "microsoft.visualstudio.qualitytools.unittestframework.dll" is denied, and I don't know how to fix this

Corner cases on covered, uncovered, redudant failed have not been thoroughly tested, and there may be issues with this.

Running tests takes over three minutes for my current test solutions, and I am not sure how to fix this.

Some methods and classes could use refactoring cleaning up

I left module and module handler classes in, because I feel like they could be used in refactoring

----------------------------------------------------------------------------------

Useful structure for console:

MsBuild structure:
From the directory of the project
MSBuild.exe Unit-Testing-Demos.sln -property:Configuration=Debug
Or
MSBuild.exe Project.csproj -property:Configuration=Debug

Opencover single test syntax:
C:\Users\Admin\.nuget\packages\opencover\4.7.922\tools\OpenCover.Console.exe 
-register:user 
-target:"C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\Common7\IDE\MSTest.exe" 
-targetargs:"/testcontainer:C:\Users\Admin\Desktop\VSTT-Demo\TestBank\bin\Debug\TestBank.dll /test:TestBank.BankTest.TestBankAccountIndexerInvalidRange /unique" 
-output:xmltest.xml -hideskipped:Filter;MissingPdb -filter:"+[*]*  -[<testnamspace>]*"

All Tests:
C:\Users\Admin\.nuget\packages\opencover\4.7.922\tools\OpenCover.Console.exe 
-register:user 
-target:"C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\Common7\IDE\MSTest.exe" 
-targetargs:"/testcontainer:C:\Users\Admin\Desktop\VSTT-Demo\TestBank\bin\Debug\TestBank.dll” 
-output:xmltest.xml 
-hideskipped:Filter;MissingPdb 
-filter:"+[*]*  
-[<testnamspace>]*"

