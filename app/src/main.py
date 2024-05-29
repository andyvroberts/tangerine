import logging
import requests
import time
import sys
import os
import ast
from calendar import monthrange
from collections import defaultdict
import store.files
import store.parquet
from ui import query as qu
import store


log = logging.getLogger("bmrs_retriever.main.py")
#------------------------------------------------------------------------------
def controller():
    """
        Orchestrate the process

        Args:
            return: None
    """
    log.info("-------------------------------------------------------------------")
    start_exec = time.time()
    log.debug(f'executing from path {os.getcwd()}')

    imbalance_prices()


    end_exec = time.time()
    duration = end_exec - start_exec
    hours, hrem = divmod(duration, 3600)
    mins, secs = divmod(hrem, 60)
    log.info(f"Finished process: {hours}:{mins}:{round(secs, 2)}.")

#---------------------------------------------------------------------------------------#  
def imbalance_prices():
    """interact with the UI to find the imbalance price function URL

        Args:
            None:
            Return: the url of the locally running function app
    """
    year, month = qu.get_settlement_month()
    year_month = f"{year}{month:02d}"
    num_days = monthrange(year, month)[1]
    price_url = qu.get_price_url()
    month_of_prices = []

    for i in range(num_days):
        run_url = (f"{price_url}/{year}/{month:02d}/{i+1:02d}")
        log.info(f'url = {run_url}')
        # get and convert the json string into a List of Dicts.
        resp = requests.get(run_url)
        daily = ast.literal_eval(resp.json().get('Value'))
        month_of_prices.extend(daily)
        time.sleep(3)

    # write out a parquet file
    store.files.write_table(store.parquet.system_prices(month_of_prices), qu.get_price_file_name(1, year_month))

#------------------------------------------------------------------------------
if __name__ == '__main__' :
    logging.basicConfig(
        level=logging.DEBUG,
        format="%(asctime)s %(levelname)-8s [%(name)s]: %(message)s",
        datefmt='%Y-%m-%d %I:%M:%S',
        handlers = [
            logging.StreamHandler(sys.stdout)
        ]
    )

    controller()