const DataBase = require('../DataBase');
const express = require('express');
const router = express.Router();
const bodyParser = require('body-parser');
router.use(bodyParser.json());

const database = new DataBase();

router.route('/').get((req, res) => {
  res.send(database.getProjects());
});

router.route('/details').post((req, res) => {
  const result = database.getProjectMonthDetails(
    req.body.user,
    req.body.code,
    req.body.month,
    req.body.year
  );
  res.send(result);
});

router.route('/allTimeDetails').post((req, res) => {
  const result = database.getProjectAllTimeDetails(req.body.code);
  res.send(result);
});

module.exports = router;
