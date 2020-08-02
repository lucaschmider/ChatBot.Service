const { UserData } = require("./UserData");

class UserCreationResult {
  /**
   * @type {UserData}
   */
  user;

  /**
   * @type {boolean}
   */
  error;

  /**
   * @type {string}
   */
  reason;

  /**
   * Creates a new instance of the UserCreationResult-Class
   * @private
   * @param {UserData} user
   * @param {string} user.uid
   * @param {string} user.name
   * @param {boolean} user.isAdmin
   * @param {string} user.department
   * @param {string} user.email
   * @param {boolean} error
   * @param {string} reason
   */
  constructor(user, error, reason) {
    this.user = user;
    this.error = error;
    this.reason = reason;
  }

  /**
   * Creates a new successful UserCreationResult
   * @param {UserData} user
   */
  static CreateForSuccess(user) {
    return new UserCreationResult(user, false, null);
  }

  /**
   * Creates a new UserCreationResult for a failed request
   * @param {string} reason
   */
  static CreateForError(reason) {
    return new UserCreationResult(null, true, reason);
  }
}

module.exports = { UserCreationResult };
