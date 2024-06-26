import azure.functions as func
from get_system_price import sysprice
from get_bmu import bmunits

app = func.FunctionApp(http_auth_level=func.AuthLevel.FUNCTION)

app.register_functions(sysprice)
app.register_functions(bmunits)
