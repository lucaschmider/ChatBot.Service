const admin = require("firebase-admin");

const getAuthToken = (req, res, next) => {
  if (req.headers.authorization && req.headers.authorization.split(" ")[0] === "Bearer") {
    req.authToken = req.headers.authorization.split(" ")[1];
  } else {
    req.authToken = null;
  }
  next();
};

const verifyIdToken = (req, res, next) => {
  getAuthToken(req, res, async () => {
    console.log(req.authToken);
    try {
      const { authToken } = req;
      const userInfo = await admin.auth().verifyIdToken(authToken);
      req.userId = userInfo.uid;
      return next();
    } catch (e) {
      console.log(e);
      return res.status(401).send({ error: "You are not authorized to make this request" });
    }
  });
};

module.exports = verifyIdToken;
