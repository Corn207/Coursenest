import { useState, useEffect } from "react";
import editIcon from "../../assets/edit-icon.png";
import deleteIcon from "../../assets/delete-icon.png";
import styles from "./ListCategories.module.css";
import instance from "../../api/request";

import Search from "../Search";
import ModalDeleteCate from "../ModalDeleteCate";
import ModalDeleteSubCate from "../ModalDeleteSubCate";
import ModalAddCate from "../ModalAddCate";
import ModalAddSubCate from "../ModalAddSubCate";
import ModalEditSubCate from "../ModalEditSubCate";
import ModalEditCate from "../ModalEditCate";
import ModalAddTopic from "../ModalAddTopic";
import ModalDeleteTopic from "../ModalDeleteTopic";
import ModalEditTopic from "../ModalEditTopic";

export default function ListCategories() {

    const [keyword, setKeyWord] = useState();
    const [data, setData] = useState([]);

    const [showModalDeleteCate, setShowModalDeleteCate] = useState(false);
    const [showModalDeleteSubCate, setShowModalDeleteSubCate] = useState(false);
    const [showModalDeleteTopic, setShowModalDeleteTopic] = useState(false);

    const [showModalAddCate, setShowModalAddCate] = useState(false);
    const [showModalAddSubCate, setShowModalAddSubCate] = useState(false);
    const [showModalAddTopic, setShowModalAddTopic] = useState(false);

    const [showModalEditSubCate, setShowModalEditSubCate] = useState(false);
    const [showModalEditCate, setShowModalEditCate] = useState(false);
    const [showModalEditTopic, setShowModalEditTopic] = useState(false);

    const [deletedData, setDeletedData] = useState({})
    const [deletedSub, setDeletedSub] = useState({});
    const [deletedTopic, setDeletedTopic] = useState({});

    const [cateId, setCateId] = useState();
    const [subcateId, setSubcateId] = useState();
    const [subcate, setSubcate] = useState({});
    const [cate, setCate] = useState({});
    const [topicNeedUpdate, setTopicNeedUpdate] = useState({});

    useEffect(() => {
        fetchListCate();
    }, []);

    const fetchListCate = () => {
        instance
            .get("categories/hierarchy")
            .then((res) => {
                setData(res.data);
            })
            .catch((err) => console.log(err));
    }
    const handleClickDelCate = (cate) => {
        setShowModalDeleteCate(true);
        setDeletedData(cate);
    };

    const handleClickDelSubCate = (sub) => {
        setShowModalDeleteSubCate(true);
        setDeletedSub(sub);
    };

    const handleClickDelTopic = (topic) => {
        setShowModalDeleteTopic(true);
        setDeletedTopic(topic);
    }
    const handleAddSubCate = (data) => {
        setShowModalAddSubCate(true);
        setCateId(data);
    }
    const handleAddTopic = (data) => {
        console.log(data);
        setShowModalAddTopic(true);
        setSubcateId(data);
    }
    const handleEditTopic = (data) => {
        setShowModalEditTopic(true);
        setTopicNeedUpdate(data);
    }
    const handleEditSubCate = (data) => {
        setShowModalEditSubCate(true);
        setSubcate(data);
    }
    const handleEditCate = (data) => {
        setShowModalEditCate(true);
        setCate(data);
    }

    return (
        <div className={styles.container}>
            <div className={styles.header}>
                <h3>Categories</h3>
                <Search setKeyWord={setKeyWord} />
                <button className={styles.buttonAdd} onClick={() => setShowModalAddCate(true)}>Add Category</button>
            </div>
            <div>
                {data && data.map((cate) => {
                    return (
                        <div className={styles.cateBox} key={cate.categoryId}>
                            <div className={styles.cateHeader}>
                                <h3>{cate.content}</h3>
                                <div className={styles.actions}>
                                    <img src={editIcon} alt="" onClick={() => handleEditCate(cate)} />
                                    <img src={deleteIcon} alt="" onClick={() => handleClickDelCate(cate)} />
                                </div>
                            </div>
                            <div className={styles.subBox}>
                                <div className={styles.subBoxHeader}>
                                    <h4>Subcategories</h4>
                                    <button className={styles.buttonAdd} onClick={() => handleAddSubCate(cate.categoryId)}>Add Subcategory</button>
                                </div>
                                <div className={styles.subBoxContent}>
                                    {
                                        cate.subcategories && cate.subcategories.map((sub) => {
                                            return (
                                                <div key={sub.subcategoryId}>
                                                    <hr className={styles.devider} />
                                                    <div className={styles.subcateContent} >
                                                        <h5>{sub.content}</h5>
                                                        <div className={styles.actions}>
                                                            <img src={editIcon} alt="" onClick={() => handleEditSubCate(sub)} />
                                                            <img src={deleteIcon} alt="" onClick={() => handleClickDelSubCate(sub)} />
                                                        </div>
                                                    </div>
                                                    <div className={styles.listTopics}>
                                                        <div className={styles.subBoxHeader}>
                                                            <h6>Topics</h6>
                                                            <button className={styles.buttonAdd} onClick={() => handleAddTopic(sub.subcategoryId)}>Add Topic</button>
                                                        </div>
                                                        <div>
                                                            {
                                                                sub.topics && sub.topics.map((topic) => {
                                                                    return (
                                                                        <div key={topic.id}>
                                                                            <hr className={styles.deviderTopic} />
                                                                            <div className={styles.topic}>
                                                                                <p>{topic.content}</p>
                                                                                <div className={styles.topicActions}>
                                                                                    <img src={editIcon} alt="" onClick={() => handleEditTopic(topic)}/>
                                                                                    <img src={deleteIcon} alt="" onClick={() => handleClickDelTopic(topic)} />
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    )
                                                                })
                                                            }
                                                        </div>
                                                    </div>
                                                </div>
                                            )
                                        })
                                    }
                                </div>
                            </div>
                        </div>
                    )
                })}
            </div>
            <ModalDeleteCate
                show={showModalDeleteCate}
                setShow={setShowModalDeleteCate}
                deletedData={deletedData}
                fetchListCate={fetchListCate}
            />
            <ModalDeleteSubCate
                show={showModalDeleteSubCate}
                setShow={setShowModalDeleteSubCate}
                deletedData={deletedSub}
                fetchListCate={fetchListCate}
            />
            <ModalDeleteTopic
                show={showModalDeleteTopic}
                setShow={setShowModalDeleteTopic}
                deletedData={deletedTopic}
                fetchListCate={fetchListCate}
            />
            <ModalAddCate
                show={showModalAddCate}
                setShow={setShowModalAddCate}
                fetchListCate={fetchListCate}
            />
            <ModalAddSubCate
                show={showModalAddSubCate}
                setShow={setShowModalAddSubCate}
                cateId={cateId}
                fetchListCate={fetchListCate}
            />
            <ModalAddTopic
                show={showModalAddTopic}
                setShow={setShowModalAddTopic}
                subcateId={subcateId}
                fetchListCate={fetchListCate}
            />
            <ModalEditSubCate
                show={showModalEditSubCate}
                setShow={setShowModalEditSubCate}
                subcate={subcate}
                fetchListCate={fetchListCate}
            />
            <ModalEditCate
                show={showModalEditCate}
                setShow={setShowModalEditCate}
                cate={cate}
                fetchListCate={fetchListCate} />
            <ModalEditTopic
                show={showModalEditTopic}
                setShow={setShowModalEditTopic}
                topicNeedUpdate={topicNeedUpdate}
                fetchListCate={fetchListCate} />
        </div>
    )
}