# Features

General summary of features based on stories

* Ability to list plates
 * Pagination - 20 per page
 * Plate, Purchase price, sale price
 * need to handle large quantity of data - 60,000,000+
   * Offset pagination or Keyset? Keyset is more performant but may not give the desired UX
 * Ability to sort
   * Sort by price - Server sort much better.
 * Ability to filter
  * filter plate based on status
    * remove reserved from results
  * Filter where plate contains text
    * alphanumeric filter
    * filter needs to handle numbers which could appear like letters
     * Ben and B3N or be45N etc
* Ability to Add a new plate to the system
    20% markup on sale price - dynamic when reading a plate, or is fixed at the point of adding a new plate to the system?
* Ability to update a plate
  * Set a satus
    * Resvered - cannot be sold
    * Sold - well its sold
  * all updates audited
* Ability to track the total revenue made from all sold plates to date
 * charts - make it look pretty, chart.js
* Ability to apply a discount code when marking  plate as sold
 * Ensure the system cannot be abused, minimum purchase price etc

# Catalog API

/plates - GET - return list of plates - Paginated
  ?Filter - string to perform filter against reg field
  ?sort - string to perform search. format: `<field>,<direction>`. 
/plates/<plateID> - GET - return individual plate
/plates/<plateID> - PUT - Update an invidual plate
/plates - POST - create a new plate

# Sales API (or add just as part of UI for speed)
/user/<userID>/basket - post - update basket. Probably just update basket as a whole rather than modify individual items in basket
/user/<userID>/basket - get - Get current basket contents
/user/<userID>/sales - get historic sales for user

If its a sales API should the `user` section exist? Maybe this isn't a sales API but a user API and the basket/sales just form part of user data.

// Possible paginated response if doing skip/take pagination
interfave PaginatedResponse {
    data: Plate[],
    totalPage: number,
    currentPage: number,
    recordsPerPage: number
}