# The task:

Create an application that retrieves public electricity data and stores aggregated data into a database. Expose API GET endpoint where could retrieve aggregated data.  
Requirements:  
• Use https://data.gov.lt/dataset/siame-duomenu-rinkinyje-pateikiami-atsitiktinai-parinktu-1000- buitiniu-vartotoju-automatizuotos-apskaitos-elektriniu-valandiniai-duomenys datasets  
• Process last two months' data  
• Filter only apartament (Butas) data  
• Store data into a database grouped by Tinklas (Regionas) field  
• For database, communication use Dapper or EF Core  
• Add logging  
• Write unit tests for the main flow  
• The app must run on docker  
