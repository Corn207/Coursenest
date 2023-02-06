import React from 'react';
import { Link } from 'react-router-dom';
import UserActions from '../UserAction/UserActions';
import styles from './Header.module.scss';
import { useEffect, useState } from 'react';
import axios from 'axios';
import images from '~/assets/images';

export default function Header() {
    const [categories, setCategories] = useState([]);
    const [subcategories, setSubcategories] = useState([]);
    const [topics, setTopics] = useState([]);
    const [isShownCategory, setIsShownCategory] = useState(false);
    const [isShownSubCategory, setIsShownSubCategory] = useState(false);
    const [isShownTopic, setIsShownTopic] = useState(false);

    useEffect(() => {
        axios
            // .get("http://192.168.0.3:21002/categories/hierarchy")
            // .get('http://localhost:3000/categories')
            .get('http://localhost:21003/categories/hierarchy')
            .then((res) => {
                setCategories(res.data);
            })
            .catch((err) => console.log(err));
    }, []);

    useEffect(() => {
        setIsShownTopic(false); 
    }, [subcategories]);

    const handleShowCategory = () => {
        setIsShownCategory(true);
    };

    const handleCloseCategory = () => {
        setIsShownCategory(false);
        setIsShownSubCategory(false);
        setIsShownTopic(false);
    };

    const handleMouseOverCate = (data) => {
        setIsShownSubCategory(true);
        setSubcategories(data);
    };

    const handleMouseOverSubCate = (data) => {
        setIsShownTopic(true);
        setTopics(data);
    };

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
                    <div 
                        className={styles.textDecoration} 
                        onClick={() => handleShowCategory()}
                    >
                        <p className={isShownCategory ? 'purpleColor' : ''}>Categories</p>
                    </div>
                    <div 
                        className={styles.dropdownMenuListItem} 
                        onMouseLeave={() => handleCloseCategory()}
                    >
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
                                                <div key={topic.id} className={styles.topic}>
                                                    <p>{topic.content}</p>
                                                </div>
                                            );
                                        })}
                                </div>
                        )}
                    </div>
                </div>
                <Link to="#" className={styles.textDecoration}>
                    <p>My Courses</p>
                </Link>
            </div>
            <div className={styles.rightHeader}>
                <img src={images.searchIcon} alt="" />
                <img src={images.publisherEditIcon} alt="" />
                <UserActions />
            </div>
        </div>
    );
}
