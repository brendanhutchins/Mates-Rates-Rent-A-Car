---
title: "Mates-Rates Rent-a-Car (MRRC) User Manual"
author: Brendan Hutchins (n10684085)
date: May 4, 2020
geometry: margin=1in
output: pdf_document
---

This program has many functionalities, and will allow the operator to do a number of processes to manage a vehicle rental management system. It is important to follow the instructions detailed in the manual carefully so that the program behaves as expected.

## Building
To build the code for this program, navigate to the `MRRC.sln` file in the root folder of the project and open it with Visual Studio. In the _Solution Explorer_ toolbar, expand the _MRRC_ solution, right-click the _MRRC_ project and select _Build_. 

## Running

To run the program press the _f5_ key on the keyboard after building the project. This will open a new console window from which the program will run. You should be met with the following greeting message, followed by the main menu:

```
    ### Mates-Rates Rent-a-Car Operation Menu ###

    You may press the ESC key at any menu to exit. Press the BACKSPACE key to return to the previous menu.
```

### Custom Files
By default, the program data is stored within appropriately labeled comma-separated value (CSV) files located in _Data_ folder (inside the root folder). Namely: 

* `fleet.csv` - Vehicle fleet data
* `customers.csv` - Customer information data
* `rentals.csv` - Vehicle and customer rental data

If the user wishes to use files stored elsewhere they should be specified as command line arguments. To do this right-click the _MRRC_ project in the _Solution Explorer_ and, select the _Debug_ tab in the newly opened file and paste the following into the _command line arguments_ text box:

```
    path/to/customers.csv path/to/fleet.csv path/to/rentals.csv 
```

Each data file must include ONLY ONE starting header line. If the data file has more than one header line, the program will crash. If the data file does not have a header line, the list created from the data folder will be missing an entire line of data.

Note that you will need to replace the dummy paths with paths to the files you wish to use in the program (relative or absolute). Once you have done this, you man run the program as described before. Make sure that the ordering on the supplied files is the exact same as the files above. 

You do not need to supply every file location, but you must supply them sequentially. For example, if you wanted to supply a fleet file, you must also supply a customer file.

## Program Usage

This program is comprised of a hierachal set of menus. You will start at the _Main Menu_, and depending on your choice you will be sent to different sub-menus, all focused on a specific aspect of the system. As specified in the greeting message you may press the `ESC` key at any menu to save the files and exit the program, and the `BACKSPACE` key to return to the previous menu. 

If you are in a function of a submenu, for example, entering a field in "Add Customer", you can input '-1' at any time to return to the previous menu, and then use the `ESC` or `BACKSPACE` keys.

### Main Menu

When you enter the program the following main menu will be displayed after the greeting message:

```
    Please enter a number from the options below:

    1) Customer Management
    2) Fleet Management
    3) Rental Management
```

After you input a number (`1`, `2` or `3`) the program will enter the corresponding sub-menu (there is no need to press enter on any of the menus, the program is expecting only a single character). The program can respond to both the top number bar of the keyboard or the NUM_PAD on the side.

### Customer Management Menu

When you enter `1` on the _Main Menu_ the following menu will be displayed. You will note that each of the options on the menu correspond to a core process relating to the management of customers.

```
    Please enter a character from the options below:

    1) Display Customers
    2) New Customer
    3) Modify Customer
    4) Delete Customer
```

After you input a character (`1`, `2`, `3` or `4`) the program will enter a small prompt based sequence to assist you in completing your task.

#### Display Customers

This sequence will very simpily draw a table to the console containing details for every customer in the CRM. It may look something like this:

```
--------------------------------------------------------
│ID │Title │FirstName │LastName    │Gender │DOB        │
--------------------------------------------------------
│0  │Ms    │Elizabeth │Franklin    │Female │26/09/1995 │
│1  │Miss  │Finley    │Sartini     │Male   │25/04/1965 │
│2  │Mr    │Miron     │Descoteaux  │Male   │04/06/1983 │
│3  │Mx    │Tekla     │Muhammad    │Other  │17/12/1989 │
│4  │Mrs   │Valentina │Waters      │Female │27/04/1979 │

```

#### New Customer

This sequence will allow the user to input a new customer to the CRM. If you want to go back to the customer menu at any time input `-1` into the Console window. It is noted that some fields, such as 'Gender', are noncase sensitive when inputting information.

```
    Please fill the following fields (fields marked with * are required)
    If You wish to go back to the previous menu, enter -1 into any field.

    ID*: Forty two
    Incorrect input detected, please try again.
    ID*: 0
    ID has been taken! Please try again.
    ID*:
```

The program also checks whether the ID supplied already exists. 

For ID, use a whole integer number. For Title, FirstName, and LastName, use any combinations of letters and even the '.', however these values cannot have any integer numbers. For DOB (date of birth), make sure to use Australian Date/Time Conventions if this is operated in another part of the world.

#### Modify Customer

This sequence allows the user to modify a customer's information. An example of how this looks is below:

```
    Please enter a customer ID to modify.
    If You wish to go back to the previous menu, enter -1 into any field.

    ID*: 0

    0│Ms│Elizabeth│Franklin│Female│26/09/1995
    1. ID                           4. LastName
    2. Title                        5. Gender
    3. FirstName                    6. DOB
    >4
    LastName*: Frinklin
    Would you like to edit another option from the vehicle? (y/n): n
```

The program will take an already existing customer ID and allow the user to modify any fields they wish. After successfully editing a field, you will be asked whether you want to continue editing the customer or not. It is imperative that if you wish to save your changes and go back to the customer menu that you enter `n` into the question field.

#### Delete Customer

This sequence allows the user to delete a customer's information from the CRM. An example of how this looks is below:

```
    Please enter a customer ID to delete.
    If You wish to go back to the previous menu, enter -1 into the field.

    ID*: 10

    10│Mr│Horece│Greenwood│Male│07/11/1999
    Are you sure you wish to delete this customer? (y/n): y
```

The ID supplied must already exist in the CRM. Additionally, the program will supply a representation of the selected customer for you to review before confirming to delete the customer. The program has safeguards to make sure that if the user accidentally types anything besides `y` or `n`, the program will return to the customer menu without deleting the customer.

The program will check if the customer is currently renting a vehicle. If the customer is currently renting a vehicle, then the customer will not be deleted until the vehicle is returned.

#### Fleet Management Menu

When you enter `2` on the _Main Menu_ the following menu will be displayed. You will note that each of the options on the menu correspond to a core process relating to the management of the vehicle fleet.

```
    Please enter a number from the options below:

    1) Display Fleet
    2) New Vehicle
    3) Modify Vehicle
    4) Delete Vehicle
```

After you input a character (`1`, `2`, `3` or `4`) the program will enter a small prompt based sequence to assist you in completing your task.

#### Display Fleet

This sequence will very simpily draw a table to the console containing details for every vehicle in the fleet. It may look something like this (if the table looks off, make sure to stretch your text reader to accompany its long size):

```
------------------------------------------------------------------------------------------------------------------------------
│Registration │Grade      │Make       │Model        │Year │NumSeats │Transmission │Fuel   │GPS   │SunRoof │DailyRate │Colour │
------------------------------------------------------------------------------------------------------------------------------
│851VOJ       │Family     │Pontiac    │Fiero 2M4    │1985 │2        │Manual       │Petrol │- 	 │-   	  │37.1      │Black  │
│169FBE       │Economy    │Pontiac    │Fiero 2M4    │1985 │2        │Manual       │Petrol │- 	 │-   	  │48.8      │Black  │
│602VVZ       │Economy    │Pontiac    │Fiero 2M4    │1987 │2        │Manual       │Petrol │- 	 │-   	  │37.9      │Red    │
│993QAN       │Commercial │Mitsubishi │3000 GT VR-4 │1991 │2        │Manual       │Petrol │- 	 │-   	  │37.8      │Red    │
```

#### New Vehicle

This sequence will allow the user to input a new vehicle to the CRM. If you want to go back to the customer menu at any time input `-1` into the Console window. An example of how this function looks is provided below:

```
    Please fill the following fields (fields marked with * are required)
    If You wish to go back to the previous menu, enter -1 into any field.

    Registration*: hei241
    Incorrect format, example format for registration is '123ABC'
    Registration*: 123abc

    Would you like to input all fields for the vehicle manually? (y/n): n
```
The program checks whether the registration supplied already exists. For vehicle registration, the format is 3 integers followed by three letters. With the exception of the 'model' field, every field is noncase sensitive.

#### Modify Vehicle

This sequence allows the user to modify a vehicle's information. If you want to go back to the customer menu at any time input `-1` into the Console window. An example of how this looks is below:

```
    Please enter a vehicle to modify.
    If You wish to go back to the previous menu, enter -1 into the field.

    Registration*: 123ABC

    123ABC│Economy│Acura│3.5 rl│1989│4│Automatic│Petrol│False│False│132.56│Brown

    Please select the option you wish to from the list below edit:

    1. Registration                 7. Transmission
    2. Grade                        8. Fuel
    3. Make                         9. GPS
    4. Model                        10. SunRoof
    5. Year                         11. DailyRate
    6. NumSeats                     12. Colour
```

The program will take an already existing vehicle registration and allow the user to modify any fields they wish. After successfully editing a field, you will be asked whether you want to continue editing the vehicle or not. It is imperative that if you wish to save your changes and go back to the customer menu that you enter `n` into the question field.

#### Delete Vehicle

This sequence allows the user to delete a customer's information from the CRM. If you want to go back to the customer menu at any time input `-1` into the Console window. An example of how this looks is below:

```
	Please enter a vehicle registration to delete.
	If You wish to go back to the previous menu, enter -1 into the field.

	Registration*: 123ABC

	123ABC│Economy│Acura│3.5 rl│1989│4│Automatic│Petrol│False│False│132.56│Brown
	Are you sure you wish to delete this vehicle? (y/n): y
```

The registration supplied must already exist in the Fleet. Additionally, the program will supply a representation of the selected vehicle for you to review before confirming to delete the vehicle. The program has safeguards to make sure that if the user accidentally types anything besides `y` or `n`, the program will return to the fleet menu without deleting the customer.

The program will check if the vehicle is currently rented. If the vehicle is currently rented, then the vehicle will not be deleted until it is returned.

### Rental Management Menu

When you enter `3` on the _Main Menu_ the following menu will be displayed. You will note that each of the options are nonfunctional, as they will be implemented in a future update.

```
	Please enter a number from the options below:

	1) Display Rentals
	2) Search Vehicles
	3) Rent Vehicle
	4) Return Vehicle
```

After you input a character (`1`, `2`, `3` or `4`) the program will enter a small prompt based sequence small prompt based sequence to assist you in completing your task.

### Display Rentals

This sequence will very simpily draw a table to the console containing details about the registration of currently rented vehicles and what customer it is rented to. It may look something like this:

```
-------------------
│Registration │ID │
-------------------
│602VVZ       │0  │
│677UIA       │6  │
│471XBI       │2  │
│851VOJ       │3  │
```

### Search Rentals

This sequence allows the user to search through a list of unrented vehicles and returns results that match the user's query. This sequence also converts the query into infix notation and also postfix notation to demonstrate how the program processes the user input. An example of how this looks is below:

```
Enter search query here, enter -1 to go back: red or green

------------Searching------------

----------------------------------------------------------------------------------------------------------------------------
│Registration │Grade      │Make       │Model        │Year │NumSeats │Transmission │Fuel   │GPS │SunRoof │DailyRate │Colour │
----------------------------------------------------------------------------------------------------------------------------
│169FBE       │Economy    │Pontiac    │Fiero 2M4    │1985 │2        │Manual       │Petrol │-   │-       │48.8      │Black  │
│993QAN       │Commercial │Mitsubishi │3000 GT VR-4 │1991 │2        │Manual       │Petrol │-   │-       │37.8      │Red    │
│682GWJ       │Commercial │Audi       │A4           │1997 │5        │Manual       │Petrol │-   │-       │46.1      │Blue   │
│519YUY       │Family     │Dodge      │Durango      │2006 │7        │Automatic    │Diesel │GPS │sunroof │46.8      │Blue   │
│123ABD       │Commercial │Honda      │Odyssey      │2009 │7        │Automatic    │Petrol │-   │sunroof │123.2     │Red    │
│642JAE       │Luxury     │Mercedes   │34-atl       │2017 │5        │Manual       │Petrol │-   │sunroof │120       │Green  │

Infix Expression...
RED OR GREEN

Postfix Expression...
RED GREEN OR

Search result:

----------------------------------------------------------------------------------------------------------------------------
│Registration │Grade      │Make       │Model        │Year │NumSeats │Transmission │Fuel   │GPS │SunRoof │DailyRate │Colour │
----------------------------------------------------------------------------------------------------------------------------
│642JAE       │Luxury     │Mercedes   │34-atl       │2017 │5        │Manual       │Petrol │-   │sunroof │120       │Green  │
│993QAN       │Commercial │Mitsubishi │3000 GT VR-4 │1991 │2        │Manual       │Petrol │-   │-       │37.8      │Red    │
│123ABD       │Commercial │Honda      │Odyssey      │2009 │7        │Automatic    │Petrol │-   │sunroof │123.2     │Red    │
```

Searching is case-insensitive and can have extra spaces anywhere. To process an attribute that has more than one word, the attribute must be surrounded by double quotes, like so:
```
"3000 GT VR-4" or green
```
The operators used include "AND" and "OR", where AND has higher priority than OR. This is illustrated below:
```
	Economy OR Family AND 4-Cylinders
					=
	Economy OR (Family AND 4-Cylinders)
```
The user can also use parenthesis to cause changes in priority or to nest values, like so:
```
	((GPS AND Sunroof) OR (Red OR Green)) AND Commercial OR Luxury
```

Any unbalanced paranthesis or double quotation marks will be caught, as well as any mispelled or nonexistent queries without crashing the program.

### Rent Vehicle

This sequence allows the user to rent a vehicle to a specific customer. If you want to go back to the rental menu at any time input `-1` into the Console window. An example of how this looks is below:

```
	Please enter a vehicle registration to rent.
	Afterward, enter a customer who will be renting that vehicle.
	If You wish to go back to the previous menu, enter -1 into the field.

	Registration*: 123abe
	ID*: 7

	Enter amount of days vehicle will be rented for: Amount of Days Vehicle is Rented*: 6
	Vehicle 123ABE successfully rented to customer 7.
	Total cost for rental is: $720.00
```

The customer provided must not already be renting a vehicle and the vehicle must not currently be rented to successfully rent the vehicle. In addition, both the vehicle and the customer must exist for the rental to take place.

The total cost is calculated by taking the vehicle's daily cost and multiplying it by the amount of days the vehicle is going to be rented. (Note that this total cost is not stored in any file, but the daily cost always is).

### Return Vehicle

This sequence allows the user to return a vehicle from a customer by using a vehicle registration. If you want to go back to the rental menu at any time input `-1` into the Console window. An example of how this looks is below:

```
	Please enter a vehicle registration to return.
	If You wish to go back to the previous menu, enter -1 into the field.

	Registration*: 851voj
	Are you sure you wish to return vehicle 851VOJ from customer 3? y/n: y
```

The vehicle must be rented to a customer for a return to be possible. In addition, the vehicle must exist in the fleet list. The program has safeguards to make sure that if the user accidentally types anything besides `y` or `n`, the program will return to the rental menu without returning the vehicle.