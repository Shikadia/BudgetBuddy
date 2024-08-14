import axios from 'axios'

console.log(process.env)
const USER_SERVICE_BASE_URL = process.env.REACT_APP_BUDGET_BUDDY_BASE_URL;
console.log(`Base URL: ${USER_SERVICE_BASE_URL}`);

const userService = axios.create({
    baseURL: USER_SERVICE_BASE_URL,
    headers: {
        "Content-Type": "application/json",
    },
    timeout: 30000,
});

const apiRequest = {
    userService
}

export default apiRequest;