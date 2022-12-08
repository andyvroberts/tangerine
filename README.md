# Tangerine
## Introduction
The Elexon BMRS is a data distribution application that provides industry data about Central Balancing and Settlement activities.  

Two of the core data series it proves are:
1. System prices
2. System volumes
  
Both of these series occure at a frequency that is 48 times (settlement periods) for every day (settlement day).  

There are two types of system price and volume:  
1. Buy price and volume
2. Sell price and volume
  
## Design
We require an application that will query the BMRS API for all data within each settlement date and write this data to 
a storage queue (where it can be further manipulated by another process).


## Technicals
[Read this file for setup and configuration](Setup.md)   
