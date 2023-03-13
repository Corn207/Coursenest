import { Routes, Route } from 'react-router-dom';
import './App.css';
import { useState } from 'react';

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
import Publisher from '~/pages/Publisher/Publisher';
import PublisherCourses from './pages/Publisher/PublisherCourses';
import AddCourses from './pages/Publisher/AddCourses';
import Topic from './pages/Topic/Topic';
import Course from './pages/Course/Course';
import axios from 'axios';
import config from './config';
import MyCourses from './pages/MyCourses/MyCourses';
import EditLesson from './pages/Publisher/AddCourses/EditLesson';
import EditMaterial from './pages/Publisher/AddCourses/EditLesson/EditMaterial';

function App() {
    let logged = false;
    const accessToken = localStorage.getItem('accessToken');
    const userId = localStorage.getItem('userId');

    const [isInstructor, setIsInstructor] = useState(false);
    const [isPublisher, setIsPublisher] = useState(false);

    const getRoleMe = () => {
        axios
            .get(`${config.baseUrl}/api/roles/me`, {
                headers: { Authorization: `Bearer ${accessToken}` },
            })
            .then((res) => {
                const roles = res.data;
                if (roles.find((role) => role.type === 1)) {
                    setIsInstructor(true);
                }
                if (roles.find((role) => role.type === 2)) {
                    setIsPublisher(true);
                }
            })
            .catch((err) => {
                console.log(err);
            });
    };
        axios.get(`${config.baseUrl}/api/roles/me`, {
            headers: { Authorization: `Bearer ${accessToken}` }
        })
            .then((res) => {
                const roles = res.data;
                if (roles.find(role => role.type === 1)) {
                    setIsInstructor(true);
                }
                if (roles.find(role => role.type === 2)) {
                    setIsPublisher(true);
                }
            })
            .catch((err) => {
                console.log(err);
            })
    }

    if (accessToken && userId) {
        logged = true;
        getRoleMe();
    }
    if (accessToken) {
        logged = true;
    }

    return (
        <div className="App">
            <Routes>
                <Route path="landing" element={<Landing />} />
                <Route path="sign-in" element={<SignIn />} />
                <Route path="sign-up" element={<SignUp />} />
                <Route path="forgot-password" element={<Forgot />} />
                <Route
                    path="/"
                    element={<Layout logged={logged} isInstructor={isInstructor} isPublisher={isPublisher} />}
                >
                    <Route index element={<Home logged={logged} />} />
                    {logged && (
                        <>
                            <Route path="profile" element={<Profile />} />
                            <Route path="courses/:id" element={<Course />} />
                            <Route path="my-courses" element={<MyCourses />} />
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
                        </>
                    )}
                    <Route path="topics/:id" element={<Topic logged={logged} />} />
                </Route>
            </Routes>
            {logged && (
                // ngoài check đã login phải check thêm role của user xem có quyền truy cập hay ko
                <Routes>
                    {isInstructor && (
                        <Route path="instructor" element={<Instructor />}>
                            <Route index element={<Following />}></Route>
                            <Route path="following" element={<Following />}></Route>
                            <Route path="pending" element={<Pending />}></Route>
                            <Route path="history" element={<History />}></Route>
                        </Route>
                    )}
                    {isPublisher && (
                        <Route path="publisher" element={<Publisher />}>
                            {/* <Route index element={<PublisherCourses />}></Route> */}
                            <Route path=":PublisherUserId" element={<PublisherCourses />}></Route>
                            {/* <Route path="courses" element={<PublisherCourses />}></Route> */}
                            <Route
                                path=":PublisherUserId/add-course"
                                element={<AddCourses isEditCourse={false} />}
                            ></Route>
                            <Route path=":PublisherUserId/add-course/add-lesson" element={<EditLesson />}></Route>
                            <Route
                                path=":PublisherUserId/add-course/add-lesson/edit-material"
                                element={<EditMaterial />}
                            ></Route>
                            <Route
                                path=":PublisherUserId/edit-course/:courseId"
                                element={<AddCourses isEditCourse={true} />}
                            ></Route>
                        </Route>
                    )}
                </Routes>
            )}
        </div>
    );
}

export default App;
