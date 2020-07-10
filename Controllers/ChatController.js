const { Router } = require("express");

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
    const data = [
      {
        message: "Das ist eine Nachricht vom Server",
        timestamp: Date.now()
      },
      {
        message: "Das ist eine andere Nachricht vom Server",
        timestamp: Date.now()
      }
    ];
    res.send(data);
  }
}

module.exports = { ChatController };
