const DataBase = require('../DataBase');
const express = require('express');
const router = express.Router();
const bodyParser = require('body-parser');
router.use(bodyParser.json());

const database = new DataBase();

router.route('/').post((req, res) => {
  const result = database.getOneUserMonth(
    req.body.user,
    req.body.month,
    req.body.year
  );
  res.send(result);
});

router.route('/freeze').post((req, res) => {
  const result = database.freezeOneUserMonth(
    req.body.user,
    req.body.month,
    req.body.year
  );
  res.send(result);
});

router.route('/setAccepted').post((req, res) => {
  const result = database.setAccepted(
    req.body.user,
    req.body.code,
    req.body.month,
    req.body.year,
    req.body.value
  );
  res.send(result);
});

module.exports = router;
