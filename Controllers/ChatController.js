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
    return router;
  }

  /**
   * Returns an array of unread messages for the requesting user.
   * @param {Request} req
   * @param {Response} res
   */
  static async GetChatMessagesAsync(req, res) {
    const data = await ChatBusiness.ReadMessagesOfUserAsync(req.userData.uid);
    res.send(data);
  }
}

module.exports = { ChatController };
