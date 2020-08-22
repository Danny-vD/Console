@echo off
if not exist "logs" mkdir logs
SET DOXYPATH=D:\Program Files\doxygen\bin\doxygen.exe

call GenerateDocumentation.bat console.arrayconverter
call GenerateDocumentation.bat console.classqueries
call GenerateDocumentation.bat console.cli
call GenerateDocumentation.bat console.core
call GenerateDocumentation.bat console.defaultconverters
call GenerateDocumentation.bat console.environmentvariables
call GenerateDocumentation.bat console.evaluator
call GenerateDocumentation.bat console.evaluator.math
call GenerateDocumentation.bat console.io
call GenerateDocumentation.bat console.networking
call GenerateDocumentation.bat console.persistentproperties
call GenerateDocumentation.bat console.propenvcompat
call GenerateDocumentation.bat console.propiocompat
call GenerateDocumentation.bat console.scriptiocompat
call GenerateDocumentation.bat console.scriptsystem
call GenerateDocumentation.bat console.utilextension
call GenerateDocumentation.bat console.unity
call GenerateDocumentation.bat console.form
call GenerateDocumentation.bat console
pause