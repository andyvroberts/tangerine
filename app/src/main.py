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

    year, month = qu.get_settlement_month()

    imbalance_prices(year, month)


    end_exec = time.time()
    duration = end_exec - start_exec
    hours, hrem = divmod(duration, 3600)
    mins, secs = divmod(hrem, 60)
    log.info(f"Finished process: {hours}:{mins}:{round(secs, 2)}.")

#---------------------------------------------------------------------------------------#  
def imbalance_prices(year, month):
    """orchestrate read and write of imbalance system prices.

        Args:
            year: of the prices
            month: of the prices
            Return: None
    """
    year_month = f"{year}{month:02d}"
    price_url = qu.get_price_url()

    month_of_prices = get_month_of_data(year, month, price_url)

    # write out a parquet file
    store.files.write_table(store.parquet.system_prices(month_of_prices), qu.get_price_file_name(1, year_month))

#---------------------------------------------------------------------------------------#  
def get_month_of_data(year, month, url):
    """for any particular retrieval, concatenate a month of data.

        Args:
            year: The settlement year
            month: the settlement month
            url: the data api base
            Return: a list of dicts representing the data collection
    """
    num_days = monthrange(year, month)[1]
    month_of_data = []

    for i in range(num_days):
        run_url = (f"{url}/{year}/{month:02d}/{i+1:02d}")
        log.debug(f'url = {run_url}')
        # get and convert the json string into a List of Dicts.
        resp = requests.get(run_url)
        daily = ast.literal_eval(resp.json().get('Value'))
        month_of_data.extend(daily)
        # do not stress the api
        time.sleep(3)

    return month_of_data

#------------------------------------------------------------------------------
if __name__ == '__main__' :
    logging.basicConfig(
        level=logging.INFO,
        format="%(asctime)s %(levelname)-8s [%(name)s]: %(message)s",
        datefmt='%Y-%m-%d %I:%M:%S',
        handlers = [
            logging.StreamHandler(sys.stdout)
        ]
    )

    controller()