import apiRequest from "./request";

const auth = {
  /**
   *
   * @param {{email: string, password: string}} data
   * @returns {AxiosResponse<{data: {id: string, roles: [], userName: string, companyName: string, token: string}, succeeded: boolean, message: string, statusCode: number}>}
   */
  login: (data) => apiRequest.userService.post("/api/v1/Auth/login", data),

   /**
   *
   * @param {{firstName: string, lastName: string, phoneNumber: string,email: string, password: string, confirmPassword: string}} data
   * @returns {AxiosResponse<{data: {id: string, roles: [], userName: string, companyName: string, token: string}, succeeded: boolean, message: string, statusCode: number}>}
   */
   signUp: (data) => apiRequest.userService.post("/api/v1/Auth/signup", data),
};

export const api = {
    auth,
};
