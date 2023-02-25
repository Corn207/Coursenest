import { Routes, Route } from 'react-router-dom';
import './App.css';

import SignIn from '~/pages/SignIn';
import SignUp from '~/pages/SignUp';
import Forgot from '~/pages/Forgot';
// import Landing from './pages/Landing Page/Landing';
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
import Topic from './pages/Topic/Topic';
import Course from './pages/Course/Course';

function App() {
    let logged = false;

    localStorage.getItem('accessToken') ? (logged = true) : (logged = false);
    return (
        <div className="App">
            <Routes>
                {/* <Route index element={<Landing />} /> */}
                <Route path="sign-in" element={<SignIn />} />
                <Route path="sign-up" element={<SignUp />} />
                <Route path="forgot-password" element={<Forgot />} />
                <Route path="/" element={<Layout logged={logged}/>}>
                    <Route index element={<Home logged={logged}/>} />
                    {logged && (
                        <>
                            <Route path="profile" element={<Profile />} />
                        </>
                    )}
                    <Route path="topics/:id" element={<Topic/>} />
                    <Route path="courses/:id" element={<Course />} />
                </Route>
            </Routes>
            {logged && (
                <Routes>
                    <Route path="instructor" element={<Instructor />}>
                        <Route index element={<Following />}></Route>
                        <Route path="following" element={<Following />}></Route>
                        <Route path="pending" element={<Pending />}></Route>
                        <Route path="history" element={<History />}></Route>
                    </Route>
                    <Route path="publisher" element={<Publisher />}>
                        <Route path=":PublisherUserId" element={<PublisherCourses />}></Route>
                        <Route path="courses" element={<PublisherCourses />}></Route>
                        <Route path="add-course" element={<AddCourses />}></Route>
                    </Route>
                </Routes>
            )}
        </div>
    );
}

export default App;
