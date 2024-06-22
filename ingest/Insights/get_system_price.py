import azure.functions as func
import calendar
import json
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

    for dd in range (1, calendar.monthrange(yyyy, mm)[1]):
        print(f'{url}{yyyy}-{mm:02d}-{dd:02d}')

    return msg