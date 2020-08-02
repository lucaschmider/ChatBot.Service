class UserData {
  uid = "";
  name = "";
  isAdmin = false;
  department = "";
  email = "";

  /**
   * Creates a new instance of the UserData-Class
   * @param {string} uid
   * @param {string} name
   * @param {boolean} isAdmin
   * @param {string} department
   * @param {string} email
   */
  constructor(uid, name, isAdmin, department, email) {
    this.uid = uid;
    this.name = name;
    this.isAdmin = isAdmin;
    this.department = department;
    this.email = email;
  }
}

module.exports = { UserData };
