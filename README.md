# Tangerine
The Elexon BMRS is a data distribution application that provides industry data about Central Balancing and Settlement activities.  

## Part 1
Two of the core data series it provides track national energy imbalance for:
1. System prices
2. System volumes
  
Both of these series usually occur at a frequency of 48 times (settlement periods) within every calendar date (settlement day).  

There are two types of system/imbalance price and volume:  
1. Buy price and volume - when there is a shortfall
2. Sell price and volume - when there is an excess
  
## Design
We require an application that will query the BMRS API for all data within each settlement date and write this data to 
a storage table (where it can be further manipulated by another process).


## Setup
[Read this file for setup and configuration](Setup.md)   
