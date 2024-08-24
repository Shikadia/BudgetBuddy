import apiRequest from "./request";

const auth = {
  /**
   *
   * @param {{email: string, password: string}} data
   * @returns {AxiosResponse<{data: {id: string, roles: [], userName: string, companyName: string, token: string}, succeeded: boolean, message: string, statusCode: number}>}
   */
  login: (data) => apiRequest.authService.post("/api/v1/Auth/login", data),

  /**
   *
   * @param {{firstName: string, lastName: string, phoneNumber: string,email: string, password: string, confirmPassword: string}} data
   * @returns {AxiosResponse<{data: {id: string, email: string,}, succeeded: boolean, message: string, statusCode: number}>}
   */
  signUp: (data) => apiRequest.authService.post("/api/v1/Auth/signup", data),

  /**
   *
   * @param {{token: string, role: string}} data
   * @returns {AxiosResponse<{data: {id: string, roles: [], userName: string, companyName: string, token: string}, succeeded: boolean, message: string, statusCode: number}>}
   */
  googleSignInUp: (data) =>
    apiRequest.authService.post("/api/v1/Auth/google-signin", data),

  /**
   *
   * @param {{currentPassword: string, newPassword: string, confirmPassword: string}} data
   * @returns {AxiosResponse<{data: {string}, succeeded: boolean, message: string, statusCode: number}>}
   */
  changePassword: (data, token) => {
    return apiRequest.userService.post("/api/v1/Auth/change-password", data, {
      headers: { Authorization: `Bearer ${token}` },
    });
  },

  /**
   *
   * @param {{id: string, refreshtoken: string}} data
   * @returns {AxiosResponse<{data: {newAccessToken: string, newRefreshToken: string}, succeeded: boolean, message: string, statusCode: number}>}
   */
  refreshToken: (data) =>
    apiRequest.authService.post("/api/v1/Auth/refresh-token", data),
  /**
   *
   * @param {{email: string, purpose: string}} data
   * @returns {AxiosResponse<{data: {string}, succeeded: boolean, message: string, statusCode: number}>}
   */
  resendOtp: (data) =>
    apiRequest.authService.post("/api/v1/Auth/resend-otp", data),
  /**
   *
   * @param {{email: string, token: string}} data
   * @returns {AxiosResponse<{data: {string}, succeeded: boolean, message: string, statusCode: number}>}
   */
  confirmEmail: (data) =>
    apiRequest.authService.post("/api/v1/Auth/confirm-email", data),
  /**
   *
   * @param {{email: string, token: string, newPassword: string, confirmPassword: string}} data
   * @returns {AxiosResponse<{data: {string}, succeeded: boolean, message: string, statusCode: number}>}
   */
  resetPassword: (data) =>
    apiRequest.authService.post("/api/v1/Auth/reset-password", data),
  /**
   *
   * @param {{email: string}} data
   * @returns {AxiosResponse<{data: {string}, succeeded: boolean, message: string, statusCode: number}>}
   */
  forgotPassword: (data) =>
    apiRequest.authService.post("/api/v1/Auth/forget-password", data),
};

const user = {
  /**
   *
   * @param {{id: string, Name: string, city: string, state: string}} data
   * @returns {AxiosResponse<{data: {string}, succeeded: boolean, message: string, statusCode: number}>}
   */
  addAddress: (data, token) => {
    return apiRequest.userService.post("/api/v1/User/add_address", data, {
      headers: { Authorization: `Bearer ${token}` },
    });
  },

  /**
   *
   * @param {pageNumber: number}
   * @param {{id: string, pageNumber: number, pageSize: number}} data
   * @returns {AxiosResponse<{ data: { pageItems: { id: string, name: string, city: string, state: string }[], pageSize: number, currentPage: number, numberOfPages: number, previousPage: number }, succeeded: boolean, message: string, statusCode: number }>}}
   */
  getAllAddress: (pageNumber, pageSize, token) => {
    const params = new URLSearchParams({
      pageNumber: pageNumber,
      pageSize: pageSize,
    });

    return apiRequest.userService.get(
      `/api/v1/User/get-all_address?${params.toString()}`,
      {
        headers: { Authorization: `Bearer ${token}` },
      }
    );
  },

  /**
   *
   * @param {pageNumber: number}
   * @param {{pageNumber: number, pageSize: number}} params
   *@returns {AxiosResponse<{ data: { listOfTransactions: { id: string, type: string, amount: number, date: string, categoryOrTag: string }[], totalAmount: number }, succeeded: boolean, message: string, statusCode: number }>}
   */
  getAllTransaction: (pageNumber, pageSize, token) => {
    const params = new URLSearchParams({
      pageNumber: pageNumber,
      pageSize: pageSize,
    });

    return apiRequest.userService.get(
      `/api/v1/User/get-all-transaction?${params.toString()}`,
      {
        headers: { Authorization: `Bearer ${token}` },
      }
    );
  },
  /**
   *
   * @param {pageNumber: number}
   * @param {{Description: string, type: string, Date: date, Amount: DeviceMotionEventAcceleration, tag: string}} data
   * @returns {AxiosResponse<{ data: { listOfTransactions: { id: string, type: string, amount: number, date: string, categoryOrTag: string }[], totalAmount: number }, succeeded: boolean, message: string, statusCode: number }>}
   */
  addTransaction: (data, pageNumber, pageSize, token) => {
    const params = new URLSearchParams({
      pageNumber: pageNumber,
      pageSize: pageSize,
    });

    return apiRequest.userService.post(
      `/api/v1/User/add-transaction?${params.toString()}`,
      data,
      {
        headers: { Authorization: `Bearer ${token}` },
      }
    );
  },
   /**
   *@param {token: string}
   * @returns {AxiosResponse<{ data: string}, succeeded: boolean, message: string, statusCode: number }>}
   */
   reset: (data,token) => {
    console.log("ddddd0", token)
    return apiRequest.userService.post(
      "/api/v1/User/reset-transactions_balance",
      data,
      {
        headers: { Authorization: `Bearer ${token}` },
      }
    );
  },
   /**
   *
   * @param {pageNumber: number}
   * @param {{id: string, Description: string, type: string, Date: date, Amount: decimal, tag: string}} data
   * @returns {AxiosResponse<{ data: { listOfTransactions: { id: string, type: string, amount: number, date: string, categoryOrTag: string }[], totalAmount: number }, succeeded: boolean, message: string, statusCode: number }>}
   */
   editTransaction: (data, token) => {
  
    return apiRequest.userService.patch(
      "/api/v1/User/edit-transaction",
      data,
      {
        headers: { Authorization: `Bearer ${token}` },
      }
    );
  },
};

export const api = {
  auth,
  user,
};
