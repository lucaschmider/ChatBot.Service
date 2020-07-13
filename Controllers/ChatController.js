const chalk = require("chalk");
const { Router } = require("express");
const { ChatBusiness } = require("../Business/ChatBusiness");

class ChatController {
  /**
   * Returns a router instance representing the ChatController
   * @returns {Router}
   */
  static getRouter() {
    const router = Router();
    router.get("/", ChatController.GetChatMessagesAsync);
    router.post("/", ChatController.SendChatMessageAsync);
    return router;
  }

  /**
   * Returns an array of unread messages for the requesting user.
   * @param {Request} req
   * @param {Response} res
   */
  static async GetChatMessagesAsync(req, res) {
    const data = await ChatBusiness.ReadMessagesOfUserAsync(req.userData.uid);
    res.setHeader("Cache-Control", "no-cache");
    res.send(data);
  }

  /**
   * Dummy. Writes a message to the queue for an echo test
   * @param {import("express").Request} req
   * @param {import("express").Response} res
   */
  static async SendChatMessageAsync(req, res) {
    try {
      await ChatBusiness.SendMessageAsync(req.userData.uid, req.body.message);
    } catch (error) {
      console.log(chalk.red("An unhandled error occured:\n"), error);
      res.status(500).send();
    }
  }
}

module.exports = { ChatController };
