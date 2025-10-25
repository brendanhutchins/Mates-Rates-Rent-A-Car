# Mates Rates Rent A Car Project

This is a C# console application I created while studying abroad at Queensland University of Technology in the Programming Principles class, completed in June, 2020. The project was made to demonstrate beginning to end UX utilizing the terminal by managing a car rental facility with its fleet inventory, customer management system, and search functionality.

Main parts of app
- Customer Relationship Management (CRM) Solution: A CRM class that has ID system, reading and writing of data to a CSV file stored on the device, LINQ querying, and multiple cases of exception handling, one being the inability to delete customers while a vehicle is rented to them.
- Fleet Inventory: This handles the vehicle inventory, rental status, and CSV read/write capabilities for recorded rentals and fleet adjustments.
- Vehicle Search: Creation of a search engine that tokenizes user queries, converting them with the shunting-yard algorithm. It then evaluates the output against hash sets and returns the matches it finds back to the user.

For information on how to operate, look on the wiki page.

![alt text](https://github.com/brendanhutchins/Mates-Rates-Rent-A-Car/blob/main/MRRC/Documentation/Photo-Class-Structure.png "Class Structure Diagram")
