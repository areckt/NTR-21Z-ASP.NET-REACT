const DataBase = require('../DataBase');
const express = require('express');
const router = express.Router();
const bodyParser = require('body-parser');
router.use(bodyParser.json());

const database = new DataBase();

router
  .route('/')
  .post((req, res) => {
    const result = database.createUser(req.body);
    res.send(result);
  })
  .get((req, res) => {
    res.send(database.getUsers());
  });

module.exports = router;
