import { faPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import classNames from 'classnames/bind';
import { useEffect, useState } from 'react';

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

    // useEffect(() => {
    //     console.log(questions);
    // }, [questions]);

    const handleAnswerChange = (questionIndex, answerIndex) => {
        setAnswers((prevAnswers) => {
            const newAnswers = [...prevAnswers];
            newAnswers[questionIndex] = answerIndex;
            return newAnswers;
        });
        setQuestions((prevQuestionList) => {
            const newQuestionList = [...prevQuestionList];
            const newQuestion = { ...newQuestionList[questionIndex] };
            newQuestion.Choice = newQuestion.Choice.map((choice, index) => {
                return {
                    ...choice,
                    IsCorrect: index === answerIndex,
                };
            });
            newQuestionList[questionIndex] = newQuestion;
            return newQuestionList;
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
    };

    const handleDeleteQuestion = (index) => {
        setQuestions((prevQuestionList) => {
            const newQuestionList = [...prevQuestionList];
            newQuestionList.splice(index, 1);
            return newQuestionList;
        });
    };

    const handleAddAnswer = (questionIndex) => {
        setQuestions((prevQuestionList) => {
            const newQuestionList = [...prevQuestionList];
            newQuestionList[questionIndex] = {
                ...newQuestionList[questionIndex],
                Choice: [
                    ...newQuestionList[questionIndex].Choice,
                    {
                        Content: 'new answer',
                        IsCorrect: false,
                    },
                ],
            };
            return newQuestionList;
        });
    };

    const handleChoiceContentEdit = (questionIndex, choiceIndex, newContent) => {
        setQuestions((prevQuestionList) => {
            const questionToUpdate = prevQuestionList[questionIndex];
            const updatedChoices = [...questionToUpdate.Choice];
            const choiceToUpdate = updatedChoices[choiceIndex];
            choiceToUpdate.Content = newContent;
            questionToUpdate.Choice = updatedChoices;
            const updatedQuestionList = [...prevQuestionList];
            updatedQuestionList[questionIndex] = questionToUpdate;
            return updatedQuestionList;
        });
    };

    const handleChoiceClick = (questionIndex, choiceIndex) => {
        const newContent = window.prompt(
            'Enter new content of the answer:',
            questions[questionIndex].Choice[choiceIndex].Content,
        );
        if (newContent !== null) {
            handleChoiceContentEdit(questionIndex, choiceIndex, newContent);
        }
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
                                <button className={cx('deleteBtn')} onClick={() => handleDeleteQuestion(questionIndex)}>
                                    X
                                </button>
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
                                        <span onClick={() => handleChoiceClick(questionIndex, answerIndex)}>
                                            {choice.Content}
                                        </span>
                                    </label>
                                </li>
                            ))}
                        </ul>
                        <button className={cx('addAnswerBtn')} onClick={() => handleAddAnswer(questionIndex)}>
                            <FontAwesomeIcon icon={faPlus} />
                        </button>
                    </li>
                ))}
            </ul>
        </div>
    );
}

export default MultiChoicesQuesTion;
