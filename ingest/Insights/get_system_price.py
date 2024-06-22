import azure.functions as func
from utils import common
import logging

blueprint001 = func.Blueprint()

@blueprint001.route(route="prices/{year}/{month}", methods=[func.HttpMethod.GET])
async def get_system_price(req: func.HttpRequest) -> str:
    _year = req.route_params.get('year')
    _month = req.route_params.get('month')
    msg = f'Get system prices for {_year}-{_month}.'

    url = 'https://data.elexon.co.uk/bmrs/api/v1/balancing/settlement/system-prices/'

    mm = int(_month)
    yyyy = int(_year)

    for date_string in common.days_in_month(yyyy, mm):
        run_url = f'{url}{date_string}'
        price_list = common.get_data_from_payload(run_url)
        print(type(price_list))
        break

    return msg