import axios from "axios";

const instance = axios.create({
	baseURL: `api/`,
});

const token = localStorage.getItem("accessToken");

instance.defaults.headers.common["Authorization"] = `Bearer ${token}`;
instance.defaults.headers.post["Content-Type"] = "application/json";
instance.defaults.headers.put["Content-Type"] = "application/json";
// instance.defaults.headers.put['Content-Type'] = "multipart/form-data";

export default instance;
