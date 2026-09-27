### CheckoutBasket

The class has more than one responsibility:

1. **Managing the basket**

   * Adding products and calculating the subtotal.

2. **Handling coupons**

   * Taking the coupon text and calculating the discount.

3. **Gift wrapping**

   * Enabling gift wrap and adding its fee to the total.

4. **Creating the gift message**

   * Generating the message that will be shown to the customer.

5. **Payment authorization**

   * Creating the payment authorization code.

### Why is this a problem?

The class is doing many different things and each one can change for a different reason For example if the coupon rules change I should not need to change the payment or gift message code So the class has more than one reason to change which violates SRP.

============================================================================================================

### AppointmentDesk

The class has more than one responsibility:

1. **Managing business hours**

   * Checks if the appointment is within the clinic's working hours.

2. **Finding available appointments**

   * Searches for the next available slot.

3. **Booking appointments**

   * Books the appointment and makes sure the same slot is not booked twice.

4. **Generating calendar events**

   * Creates an ICS calendar event for the appointment.

5. **Sending appointment reminders**

   * Creates an SMS reminder message for the patient.

### Why is this a problem?

The class is responsible for different things For example changing the clinic working hours is different from changing the SMS message or the calendar format Each responsibility can change for a different reason so they should not all be inside the same class.

============================================================================================================

### CourseEnrollmentDesk

The class has more than one responsibility:

1. **Course registration**

   * Registers students in the course and checks if they are already registered.

2. **Waitlist management**

   * Adds students to the waitlist, finds their position, and promotes them when a seat is available.

3. **Welcome message**

   * Creates a welcome message for the student.

4. **Invoice calculation**

   * Calculates tuition, VAT, and the total amount.

5. **Course capacity**

   * Checks the available seats and prevents the course from exceeding its capacity.

### Why is this a problem?

These responsibilities can change for different reasons. For example, changing the invoice or VAT calculation should not require changing the registration or waitlist logic. So, they should be separated into different classes.

============================================================================================================

### GradeBook

The class has more than one responsibility:

1. **Recording grades**

   * Stores the scores for each student.

2. **Calculating average**

   * Calculates the average score for a student.

3. **Grading policy**

   * Determines the letter grade based on the student's average.

4. **Checking honor roll**

   * Checks if the student meets the honor roll requirements.

5. **Exporting student information**

   * Creates a transcript and exports the grades as CSV.

### Why is this a problem?

The class handles grading, academic rules, and exporting data at the same time. These things can change for different reasons. For example, changing the grading rules should not require changing the CSV export format. So, these responsibilities should be separated.

============================================================================================================

### KitchenTicket

The class has more than one responsibility:

1. **Managing order items**

   * Adds and stores the items with their ingredients and preparation time.

2. **Detecting allergens**

   * Checks the ingredients and detects possible allergens.

3. **Calculating preparation time**

   * Estimates how long the order will take to be ready.

4. **Preparing the kitchen ticket**

   * Creates the ticket format that will be printed.

5. **Choosing the order lane**

   * Decides if the order should go to the allergy, slow, or fast lane.

### Why is this a problem?

The class handles different responsibilities like allergen detection, time calculation, and printing the ticket. These responsibilities can change for different reasons. For example, changing the printer format should not require changing the allergen detection rules.

============================================================================================================

### LoanDesk

The class has more than one responsibility:

1. **Risk calculation**

   * Calculates the risk score for the loan request.

2. **Eligibility check**

   * Checks if the applicant is eligible for the loan.

3. **Required documents**

   * Determines which documents are required from the applicant.

4. **Decision letter**

   * Creates a letter with the approval or rejection decision.

5. **CSV export**

   * Exports the application information as a CSV row.

### Why is this a problem?

The class handles risk calculation, document requirements, decision messages, and data export. These responsibilities can change for different reasons. For example, changing the risk calculation should not require changing the decision letter or CSV format.

============================================================================================================

### SubscriptionBilling

The class has more than one responsibility:

1. **Proration calculation**

   * Calculates the amount that the customer should pay based on the subscription period.

2. **Invoice number generation**

   * Generates the next invoice number.

3. **Failed payment tracking**

   * Keeps track of failed payments.

4. **Dunning email**

   * Creates an email to notify the customer about the payment problem.

5. **Accounting ledger**

   * Creates a line containing the billing information for the accounting ledger.

### Why is this a problem?

The class handles billing calculations, invoice numbers, payment tracking, emails, and accounting export. These responsibilities can change for different reasons. For example, changing the invoice numbering system should not require changing the proration calculation or email message.

============================================================================================================

### SupportTicket

The class has more than one responsibility:

1. **Managing the ticket**

   * Stores the ticket information and adds new customer messages.

2. **Calculating priority**

   * Checks the ticket text and determines the priority.

3. **Calculating SLA deadline**

   * Calculates the deadline based on the ticket priority.

4. **Checking SLA breach**

   * Checks if the SLA deadline has been exceeded.

5. **Creating customer reply**

   * Creates a message to reply to the customer.

6. **Internal escalation**

   * Creates an internal message when the ticket needs escalation.

### Why is this a problem?

The class handles ticket management, priority rules, SLA calculations, and different types of messages. These responsibilities can change for different reasons. For example, changing the customer reply template should not require changing the SLA rules.

============================================================================================================

### WardBoard

The class has more than one responsibility:

1. **Managing beds and patients**

   * Assigns patients to beds and stores their information.

2. **Calculating acuity score**

   * Calculates the patient's acuity score based on vital signs.

3. **Managing pager alerts**

   * Decides when a pager alert should be fired and stores the alerts.

4. **Creating handoff notes**

   * Creates notes that can be used when handing over the patient.

5. **Exporting data**

   * Exports the ward information as a CSV file.

### Why is this a problem?

The class handles patient and bed management, clinical calculations, pager alerts, notes, and data export. These responsibilities can change for different reasons. For example, changing the CSV format should not require changing the acuity calculation or pager rules.

============================================================================================================

### WarehousePickList

The class has more than one responsibility:

1. **Stock allocation**

   * Calculates how many items can be allocated based on the required and available quantity.

2. **Walking order**

   * Creates the order in which the picker should visit the aisles and bins.

3. **Picker instructions**

   * Creates instructions for the picker and shows any shortages.

4. **WMS XML export**

   * Exports the allocated items as XML for the warehouse management system.

### Why is this a problem?

The class handles stock allocation, warehouse path ordering, picker instructions, and XML integration. These responsibilities can change for different reasons. For example, changing the warehouse layout should not require changing the XML format or stock allocation rules.
============================================================================================================


