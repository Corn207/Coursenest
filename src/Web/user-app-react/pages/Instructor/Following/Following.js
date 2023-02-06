import classNames from 'classnames/bind';

import FollowingSubject from '~/components/FollowingSubject';
import styles from './Following.module.scss';

const cx = classNames.bind(styles);

function Following() {
    const topics = [
        {
            TopicId: 1,
            Content: 'Topic number 1',
            CategoryContent: 'CategoryContent number 1',
            SubcategoryContent: 'SubcategoryContent number 1',
        },
        {
            TopicId: 2,
            Content: 'Topic number 2',
            CategoryContent: 'CategoryContent number 1',
            SubcategoryContent: 'SubcategoryContent number 1',
        },
        {
            TopicId: 3,
            Content: 'Topic number 3',
            CategoryContent: 'CategoryContent number 1',
            SubcategoryContent: 'SubcategoryContent number 1',
        },
        {
            TopicId: 4,
            Content: 'Topic number 4',
            CategoryContent: 'CategoryContent number 1',
            SubcategoryContent: 'SubcategoryContent number 1',
        },
    ];

    return (
        <>
            <div className={cx('wrapper')}>
                <div className={cx('followingContainer')}>
                    <p className={cx('mainTab')}>Following</p>
                    <FollowingSubject title={'Topics'} items={topics} type={'Topic'} />
                    <FollowingSubject title={'Sub-Categories'} items={topics} type={'Topic'} />
                    <FollowingSubject title={'Categories'} items={topics} type={'Topic'} />
                    <FollowingSubject title={'Courses'} items={topics} type={'Topic'} />
                </div>
                <div>Search</div>
            </div>
        </>
    );
}

export default Following;
