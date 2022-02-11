const path = require('path');
const express = require('express');
const cors = require('cors');
const app = express();

app.listen(3001);
app.use(cors());

const userRouter = require('./routes/users');
app.use('/users', userRouter);

const projectsRouter = require('./routes/projects');
app.use('/projects', projectsRouter);

const activitiesRouter = require('./routes/activities');
app.use('/activities', activitiesRouter);

const monthsRouter = require('./routes/months');
app.use('/months', monthsRouter);

app.use(express.static(path.join(__dirname, '../front/build')));
app.get('/', (req, res) => {
  res.sendFile(path.join(__dirname, '../front/build', 'index.html'));
});
