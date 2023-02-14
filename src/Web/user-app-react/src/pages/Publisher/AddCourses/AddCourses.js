import classNames from 'classnames/bind';
import { useState } from 'react';
import LessonsMaterialLists from '~/components/PublisherPage/LessonsMaterialLists';

import styles from './AddCourses.module.scss';

const cx = classNames.bind(styles);

function AddCourses() {
    const [image, setImage] = useState(null);
    const [isSelected, setIsSlected] = useState(1);

    const handleChange = (e) => {
        setImage(URL.createObjectURL(e.target.files[0]));
    };

    const handleClick = (id) => {
        setIsSlected(id);
    };

    return (
        <div className={cx('wrapper')}>
            <p className={cx('title')}>Web Design Course</p>
            <div className={cx('contentContainer')}>
                <div className={cx('imageDiv')}>
                    <div className={cx('imageActionDiv')}>
                        <p className={cx('fileTitle')}>Course Image</p>
                        <input type="file" onChange={handleChange} />
                    </div>
                    {image && <img className={cx('imageContainer')} src={image} alt={'course'}></img>}
                </div>
                <div className={cx('courseInfoDiv')}>
                    <div className={cx('courseInputInfo')}>
                        <p className={cx('inputTitle')}>Course Title</p>
                        <textarea className={cx('input')} type={'text'} placeholder={'Title name...'}></textarea>
                    </div>
                    <div className={cx('courseInputInfo')}>
                        <p className={cx('inputTitle')}>Description</p>
                        <textarea className={cx('input')} type={'text'} placeholder={'Description...'}></textarea>
                    </div>
                    <div className={cx('courseInputInfo')}>
                        <p className={cx('inputTitle')}>Course Tier</p>
                        <div className={cx('courseTierDiv')}>
                            <p
                                className={cx('courseTier')}
                                style={{ opacity: isSelected === 1 ? 1 : 0.3 }}
                                onClick={() => handleClick(1)}
                            >
                                Free
                            </p>
                            <p
                                className={cx('courseTier')}
                                style={{ opacity: isSelected === 2 ? 1 : 0.3 }}
                                onClick={() => handleClick(2)}
                            >
                                Premium
                            </p>
                        </div>
                    </div>
                    <div className={cx('courseInputInfo')}>
                        <p className={cx('inputTitle')}>About</p>
                        <textarea
                            className={cx('inputAbout')}
                            type={'text'}
                            placeholder={'Write something...'}
                        ></textarea>
                    </div>
                </div>

                <div className={cx('lessonsContainer')}>
                    <div className={cx('topContainer')}>
                        <p className={cx('topTitle')}>Lessons</p>
                        <button className={cx('topTitleBtn')}>Add Criteria</button>
                    </div>
                    <LessonsMaterialLists />
                </div>
            </div>
        </div>
    );
}

export default AddCourses;
