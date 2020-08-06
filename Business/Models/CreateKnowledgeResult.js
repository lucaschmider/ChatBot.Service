class CreateKnowledgeResult {
  /**
   * @type {boolean}
   */
  error;

  /**
   * @type {string}
   */
  reason;

  /**
   * Creates a new instance of the CreateKnowledgeResult-Class
   * @private
   * @param {boolean} error
   * @param {string} reason
   */
  constructor(error, reason) {
    this.error = error;
    this.reason = reason;
  }

  /**
   * Creates a new successful CreateKnowledgeResult
   */
  static CreateForSuccess() {
    return new CreateKnowledgeResult(false, null);
  }

  /**
   * Creates a new CreateKnowledgeResult for a failed request
   * @param {string} reason
   */
  static CreateForError(reason) {
    return new CreateKnowledgeResult(true, reason);
  }
}

module.exports = { CreateKnowledgeResult };
