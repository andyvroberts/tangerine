import azure.functions as func
from get_system_price import blueprint001 as bp1
import datetime
import json
import logging

app = func.FunctionApp(http_auth_level=func.AuthLevel.FUNCTION)

app.register_functions(bp1)