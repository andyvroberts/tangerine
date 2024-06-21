import azure.functions as func
import json
import logging

blueprint001 = func.Blueprint()

@blueprint001.route(route="prices/{settlementdate}", methods=[func.HttpMethod.GET])
async def get_system_price(req: func.HttpRequest):
    sett_date = req.route_params.get('settlementdate')
    logging.info(f'Get system prices for {sett_date}.')
