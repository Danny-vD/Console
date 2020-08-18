if not exist "%1" mkdir %1
echo Generating Documentation for Project: %1
"%DOXYPATH%" %1.cfg>logs\%1.log