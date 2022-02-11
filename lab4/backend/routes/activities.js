const DataBase = require('../DataBase');
const express = require('express');
const router = express.Router();
const bodyParser = require('body-parser');
router.use(bodyParser.json());

const database = new DataBase();

router.route('/').get((req, res) => {
  res.send(database.getActivities());
});

router.route('/oneUser').post((req, res) => {
  const result = database.getOneUserActivities(req.body);
  res.send(result);
});

router.route('/add').post((req, res) => {
  const result = database.addActivity(
    req.body.user,
    req.body.day,
    req.body.month,
    req.body.year,
    req.body.code,
    req.body.subcode,
    req.body.time,
    req.body.description
  );
  res.send(result);
});

router.route('/delete').post((req, res) => {
  const result = database.deleteActivity(req.body.id);
  res.send(result);
});

module.exports = router;
