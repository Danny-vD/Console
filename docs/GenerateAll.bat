@echo off
if not exist "logs" mkdir logs
SET DOXYPATH=D:\Program Files\doxygen\bin\doxygen.exe

call GenerateDocumentation.bat compat.evaluator.vars
call GenerateDocumentation.bat compat.script.io
call GenerateDocumentation.bat console.cli
call GenerateDocumentation.bat console.core
call GenerateDocumentation.bat console.evaluator
call GenerateDocumentation.bat console.evaluator.math
call GenerateDocumentation.bat console.form
call GenerateDocumentation.bat console.io
call GenerateDocumentation.bat console.namespaces
call GenerateDocumentation.bat console.networking
call GenerateDocumentation.bat console.persistence
call GenerateDocumentation.bat console.script
call GenerateDocumentation.bat console.unity
call GenerateDocumentation.bat console.utility
call GenerateDocumentation.bat console.vars
call GenerateDocumentation.bat console
pause