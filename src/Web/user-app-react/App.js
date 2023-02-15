import { Routes, Route } from 'react-router-dom';
import './App.css';

import SignIn from '~/pages/SignIn';
import SignUp from '~/pages/SignUp';
import Forgot from '~/pages/Forgot';

import Landing from './pages/Landing Page/Landing';
import Home from './pages/Home/Home';
import Profile from './pages/Profile/Profile';
import Layout from './pages/Layout/Layout';
import Instructor from '~/pages/Instructor';
import Following from '~/pages/Instructor/Following';
import Pending from '~/pages/Instructor/Pending';
import History from '~/pages/Instructor/History';

function App() {
    const accessToken = localStorage.getItem('accessToken');

    return (
        <div className="App">
            {!accessToken && (
                <>
                    <Routes>
                        <Route index element={<SignIn />} />
                        <Route path="login" element={<SignIn />} />
                    </Routes>
                </>
            )}
            {accessToken && (
                <Routes>
                    <Route path="/" element={<Layout />}>
                        <Route index element={<Home />} />
                        <Route path="profile" element={<Profile />} />
                    </Route>
                    <Route path="sign-up" element={<SignUp />} />

                    <Route path="forgot-password" element={<Forgot />} />
                    {/* <Route path="reset-password" element={<ResetPassword />} /> */}
                    <Route path="/landing-page" element={<Landing />} />
                    <Route path="instructor" element={<Instructor />}>
                        <Route index element={<Following />}></Route>
                        <Route path="following" element={<Following />}></Route>
                        <Route path="pending" element={<Pending />}></Route>
                        <Route path="history" element={<History />}></Route>
                    </Route>
                    <Route path="*" element={<p>Path not resolved</p>} />
                </Routes>
            )}
        </div>
    );
}

export default App;
