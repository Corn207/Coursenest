import { faPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import classNames from 'classnames/bind';
import { useState } from 'react';

import styles from './MultiChoicesQuesTion.module.scss';

const cx = classNames.bind(styles);

// const questionList = [
//     {
//         Content: 'question a',
//         Point: 1,
//         Choice: [
//             {
//                 Content: 'answer 1',
//                 IsCorrect: false,
//             },
//             {
//                 Content: 'answer 2',
//                 IsCorrect: false,
//             },
//             {
//                 Content: 'answer 3',
//                 IsCorrect: true,
//             },
//         ],
//     },
//     {
//         Content: 'question b',
//         Point: 1,
//         Choice: [
//             {
//                 Content: 'answer 1',
//                 IsCorrect: true,
//             },
//             {
//                 Content: 'answer 2',
//                 IsCorrect: false,
//             },
//             {
//                 Content: 'answer 3',
//                 IsCorrect: false,
//             },
//         ],
//     },
//     {
//         Content: 'question c',
//         Point: 1,
//         Choice: [
//             {
//                 Content: 'answer 1',
//                 IsCorrect: false,
//             },
//             {
//                 Content: 'answer 2',
//                 IsCorrect: true,
//             },
//             {
//                 Content: 'answer 3',
//                 IsCorrect: false,
//             },
//         ],
//     },
// ];

function MultiChoicesQuesTion({ title, addBtnName }) {
    const [questions, setQuestions] = useState([]);
    const [answers, setAnswers] = useState([]);

    const handleAnswerChange = (questionIndex, answerIndex) => {
        setAnswers((prevAnswers) => {
            const newAnswers = [...prevAnswers];
            newAnswers[questionIndex] = answerIndex;
            return newAnswers;
        });
    };

    const handleAddLessonClick = (event) => {
        event.preventDefault();
        if (questions.length === 0) {
            const defaultNewQuestion = {
                // LessonId: 1,
                Content: `New item 1`,
                Point: 1,
                Choice: [
                    {
                        Content: 'answer 1',
                        IsCorrect: true,
                    },
                    {
                        Content: 'answer 2',
                        IsCorrect: false,
                    },
                    {
                        Content: 'answer 3',
                        IsCorrect: false,
                    },
                ],
            };
            const addedQuestionsList = [defaultNewQuestion];
            setQuestions(addedQuestionsList);
        } else {
            const defaultNewQuestion = {
                // LessonId: lessons[lessons.length - 1].LessonId + 1,
                Content: `New item ${questions.length + 1}`,
                Point: 1,
                Choice: [
                    {
                        Content: 'answer 1',
                        IsCorrect: true,
                    },
                    {
                        Content: 'answer 2',
                        IsCorrect: false,
                    },
                    {
                        Content: 'answer 3',
                        IsCorrect: false,
                    },
                ],
            };
            const addedQuestionsList = [...questions, defaultNewQuestion];
            setQuestions(addedQuestionsList);
        }
        // setLessons(addedLessonsList);
        console.log(questions);
    };

    return (
        <div className={cx('wrapper')}>
            <div className={cx('top')}>
                <p className={cx('title')}>{title}</p>
                <button className={cx('addQuestionBtn')} onClick={handleAddLessonClick}>
                    {addBtnName}
                </button>
            </div>
            <ul className={cx('body')}>
                {questions.map((item, questionIndex) => (
                    <li className={cx('question')} key={questionIndex}>
                        <div className={cx('questionHeader')}>
                            <p className={cx('questionName')}>
                                Question {questionIndex + 1}: {item.Content}
                            </p>
                            <div className={cx('rightHeader')}>
                                <p className={cx('point')}>{item.Point} Point</p>
                                <button className={cx('deleteBtn')}>X</button>
                            </div>
                        </div>
                        <ul className={cx('answerList')}>
                            {item.Choice.map((choice, answerIndex) => (
                                <li key={answerIndex} className={cx('answerLi')}>
                                    <label className={cx('answerLabel')}>
                                        <input
                                            className={cx('answerInput')}
                                            type="radio"
                                            name={`question-${questionIndex}`}
                                            value={choice.Content}
                                            checked={answers[questionIndex] === answerIndex}
                                            onChange={() => handleAnswerChange(questionIndex, answerIndex)}
                                        />
                                        {choice.Content}
                                    </label>
                                </li>
                            ))}
                        </ul>
                        <button className={cx('addAnswerBtn')}>
                            <FontAwesomeIcon icon={faPlus} />
                        </button>
                    </li>
                ))}
            </ul>
        </div>
    );
}

export default MultiChoicesQuesTion;
