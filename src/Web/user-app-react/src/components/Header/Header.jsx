import React from 'react';
import { Link } from 'react-router-dom';
import UserActions from '../UserAction/UserActions';
import styles from './Header.module.scss';
import images from '../../assets/images';
import GuestActions from '../UserAction/GuestActions';

export default function Header(props) {
    const {
        logged,
        categories,
        subcategories,
        topics,
        handleClickTopic,
        handleShowCategory,
        handleCloseCategory,
        handleMouseOverCate,
        handleMouseOverSubCate,
        isShownCategory,
        isShownSubCategory,
        isShownTopic,
    } = props;

    return (
        <div className={styles.container}>
            <style>{`
                .purpleColor {
                    color: #C677FC;
                }
            `}</style>

            <div className={styles.leftHeader}>
                <img src={images.purpleLogo} alt="" />
                <div className={styles.dropdownMenu}>
                    <div className={styles.textDecoration} onClick={() => handleShowCategory()}>
                        <p className={isShownCategory ? 'purpleColor' : ''}>Categories</p>
                    </div>
                    <div className={styles.dropdownMenuListItem} onMouseLeave={() => handleCloseCategory()}>
                        {isShownCategory && (
                            <div className={styles.dropdownListCate}>
                                {categories &&
                                    categories.map((category) => {
                                        return (
                                            <div
                                                key={category.categoryId}
                                                className={styles.dropdownItemCate}
                                                onMouseOver={() => handleMouseOverCate(category.subcategories)}
                                            >
                                                <p>{category.content}</p>
                                                <p>&gt;</p>
                                            </div>
                                        );
                                    })}
                            </div>
                        )}
                        {isShownSubCategory && (
                            <div className={styles.dropdownListItemSub}>
                                <h5>Sub-Categories</h5>
                                {subcategories &&
                                    subcategories.map((subcategory) => {
                                        return (
                                            <div
                                                key={subcategory.subcategoryId}
                                                className={styles.dropdownItemSubCate}
                                                onMouseOver={() => handleMouseOverSubCate(subcategory.topics)}
                                            >
                                                <p>{subcategory.content}</p>
                                                <p>&gt;</p>
                                            </div>
                                        );
                                    })}
                            </div>
                        )}
                        {isShownTopic && (
                            <div className={styles.listTopics}>
                                <h5>Popular topics</h5>
                                {topics &&
                                    topics.map((topic) => {
                                        return (
                                            <div
                                                key={topic.id}
                                                className={styles.topic}
                                                onClick={() => handleClickTopic(topic)}
                                            >
                                                <p>{topic.content}</p>
                                            </div>
                                        );
                                    })}
                            </div>
                        )}
                    </div>
                </div>
                {logged && (
                    <Link to="my-courses" className={styles.textDecoration}>
                        <p>My Courses</p>
                    </Link>
                )}
            </div>
            <div className={styles.rightHeader}>
                { logged && (
                <>
                    <img className={styles.searchIcon} src={images.searchIcon} alt="" />
                    <img className={styles.publisherEditIcon} src={images.publisherEditIcon} alt="" />
                    <img className={styles.instructorAction} src={images.instructorAction} alt="" />
                    <UserActions />
                </>
                )}
                { !logged && (<GuestActions />)}
            </div>
        </div>
    );
}
