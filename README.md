# Accounting Manager Website
This is a work in progress C# ASP.NET 7 personal website to view and edit my accounting data. All data is stored in a local SQLite file and accessed via Entity Framework Core.

<ins>**This project is designed to be run on my local network as a learning experience, so security and efficency are not a priority.**</ins>

## Current Features
- Standard CRUD operations for transactions.
- Ability to import transactions into the system view a csv bank statement:
<p align="center">
  <img src="https://i.gyazo.com/06005cea6080101b702be5c9128fa245.png" width="600">
</p>

- The table loads transactions in as you scroll down the page.
- Searchable fields for all table columns:
<p align="center">
  <img src="https://i.gyazo.com/48209797f5660e56e78b98012d210f27.png" width="600">
</p>

- Ability to sort any column descending or ascending:
<p align="center">
  <img src="https://i.gyazo.com/46a7661019cdde01615bd791881dfe9d.png" width="600">
</p>

- Receipts can be added and linked to a transaction:
<p align="center">
  <img src="https://i.gyazo.com/8dc3b7cb14997878ca997729039c9a7a.png" width="600">
</p>

- View various reports regarding your transactions:
<p align="center">
  <img src="https://i.gyazo.com/980fb8f4a3addfad6657011783b68dc4.png" width="600">
</p>

- Create items and different measurments to use with the items:
<p align="center">
  <img src="https://i.gyazo.com/5358bbe7e3ccdffe68bf0abba2ace898.png" width="600">
</p>

- Assign items to a transaction to improve tracking and reporting:
<p align="center">
  <img src="https://i.gyazo.com/b677462e9a2eac1b9fe9e784cea84f78.png" width="600">
</p>

- Various themes that can be selected from a dropdown and are saved to the browser's cookies:
<p align="center">
  <img src="https://i.gyazo.com/c24dc2a9c13fed47adefcaeb085e54a6.png" width="600">
</p>
<p align="center">
  <img src="https://i.gyazo.com/1dbeac54d77cfc79e696d6192df7e62a.png" width="600">
</p>

## Planned Features
- Improve upon reporting
- Add in automated receipt scanning to determine the items within a transaction
