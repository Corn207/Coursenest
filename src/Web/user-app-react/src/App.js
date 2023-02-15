import { Routes, Route } from 'react-router-dom';
import './App.css';

import SignIn from '~/pages/SignIn';
import SignUp from '~/pages/SignUp';
import Forgot from '~/pages/Forgot';
import ResetPassword from '~/pages/ResetPassword';

import Landing from './pages/Landing Page/Landing';
import Home from './pages/Home/Home';
import Profile from './pages/Profile/Profile';
import Layout from './pages/Layout/Layout';
import Instructor from '~/pages/Instructor';
import Following from '~/pages/Instructor/Following';
import Pending from '~/pages/Instructor/Pending';
import History from '~/pages/Instructor/History';
import Publisher from '~/pages/Publisher/Publisher';
import PublisherCourses from './pages/Publisher/PublisherCourses';
import AddCourses from './pages/Publisher/AddCourses';

function App() {
    let logged = false;

    localStorage.getItem('accessToken') ? (logged = true) : (logged = false);
    return (
        <div className="App">
            {!logged && (
                <>
                    <Routes>
                        <Route index element={<SignIn />} />
                        <Route path="login" element={<SignIn />} />
                    </Routes>
                </>
            )}
            {logged && (
                <Routes>
                    <Route path="/" element={<Layout />}>
                        <Route index element={<Home />} />
                        <Route path="profile" element={<Profile />} />
                    </Route>
                    <Route path="sign-up" element={<SignUp />} />

                    <Route path="forgot-password" element={<Forgot />} />
                    <Route path="reset-password" element={<ResetPassword />} />
                    <Route path="/landing-page" element={<Landing />} />
                    <Route path="instructor" element={<Instructor />}>
                        <Route index element={<Following />}></Route>
                        <Route path="following" element={<Following />}></Route>
                        <Route path="pending" element={<Pending />}></Route>
                        <Route path="history" element={<History />}></Route>
                    </Route>
                    <Route path="publisher" element={<Publisher />}>
                        {/* <Route index element={<PublisherCourses />}></Route> */}
                        <Route path=":PublisherUserId" element={<PublisherCourses />}></Route>
                        <Route path="courses" element={<PublisherCourses />}></Route>
                        <Route path="add-course" element={<AddCourses />}></Route>
                    </Route>
                    <Route path="*" element={<p>Path not resolved</p>} />
                </Routes>
            )}
        </div>
    );
}

export default App;
