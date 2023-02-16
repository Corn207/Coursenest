import axios from 'axios';
import { useEffect, useState } from 'react';
import AllCoursesByTopic from '~/components/AllCoursesByTopic/AllCoursesByTopic';
import config from '~/config';
import styles from './Home.module.scss';

export default function Home() {
    // test
    const userId = 19;
    const [topics, setTopics] = useState([]);
    const [activeTopic, setActiveTopic] = useState([]);
    const [listCourses, setListCourses] = useState([]);

    useEffect(() => {
        axios
            .get(`${config.baseUrl}/api/users/${userId}`)
            .then((res) => {
                console.log(res.data);
                setTopics(res.data.interestedTopics);
                return res.data.interestedTopics;
            })
            .then(async (res) => {
                const allInterestedTopics = [];
                await Promise.all(
                    res.map(async (id) => {
                        const response = await axios.get(`${config.baseUrl}/api/topics/${id}`);
                        const topic = response.data;
                        allInterestedTopics.push(topic);
                    }),
                );
                console.log(allInterestedTopics);
                setTopics(allInterestedTopics);
                setActiveTopic(allInterestedTopics[0]);
            })
            .catch((err) => {
                console.log(err);
            });
    }, []);

    useEffect(() => {
        axios
            .get(
                `${config.baseUrl}/api/courses?TopicId=${
                    activeTopic.topicId
                }&IsApproved=true&SortBy=0&PageNumber=${1}&PageSize=${5}`,
            )
            .then((res) => {
                setListCourses(res.data);
                console.log(res.data);
            })
            .catch((err) => {
                console.log(err);
            });
    }, [activeTopic]);

    const handleClickTopic = (data) => {
        setActiveTopic(data);
    };

    return (
        <div className={styles.container}>
            <div className={styles.slogan}>
                <h1 className={styles.title}># ONLINE COURSE EXAM</h1>
                <span className={styles.subTitle}>BY TEAM 3</span>
            </div>

            <div>
                <h3 className={styles.heading}>Most Popular Topics</h3>
                <div className={styles.listInterestedTopic}>
                    {topics &&
                        topics.map((topic) => {
                            return (
                                <div
                                    className={styles.interestedTopic}
                                    key={topic.topicId}
                                    onClick={() => {
                                        handleClickTopic(topic);
                                    }}
                                >
                                    {topic.content}
                                </div>
                            );
                        })}
                </div>
                <h3 className={styles.heading}>Top Courses</h3>
                <div>
                    <AllCoursesByTopic listCourses={listCourses.courses} />
                </div>
            </div>
        </div>
    );
}
