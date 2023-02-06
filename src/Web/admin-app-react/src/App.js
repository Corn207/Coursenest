import { Routes, Route, Outlet } from "react-router-dom";
import 'bootstrap/dist/css/bootstrap.min.css';

import Header from "./components/Header/Header";
import ManageUsers from "./components/ManageUsers/ManageUsers";

function App() {
    return (
        <div>
            <Header />
            <Outlet />
            <Routes>
                <Route exact path="/" element={<ManageUsers />} />
                <Route path="*" element={<p>Path not resolved</p>} />
            </Routes>
        </div>
    );
}
export default App;