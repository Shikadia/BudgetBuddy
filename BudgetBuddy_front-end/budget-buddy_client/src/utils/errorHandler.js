import { toast } from "react-toastify";

export const handleApiErrors = (error) => {
  if (error.response) {
    const { status, data } = error.response;

    switch (status) {
      case 400:
        if (data.errors) {
          Object.values(data.errors).forEach((messages) => {
            messages.forEach((message) => toast.error(message));
          });
        } else {
          toast.error(data.message || "Bad Request");
        }
        break;

      case 401:
        toast.error("Unauthorized. Please log in.");
        break;

      case 403:
        toast.error(
          "Forbidden. You do not have permission to perform this action."
        );
        break;

      case 404:
        toast.error("Resource not found.");
        break;

      case 500:
        toast.error("Internal Server Error. Please try again later.");
        break;

      default:
        toast.error(data.message || "An error occurred.");
        break;
    }
  } else if (error.request) {
    if (error.code === "ECONNABORTED") {
      toast.error("Request timed out. Please try again.");
    } else {
      toast.error("Network error. Please check your internet connection.");
    }
  } else {
    toast.error("An unexpected error occurred.");
  }
};
