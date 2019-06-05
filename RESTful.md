# RESTful

## HTTP method

- GET

  | 请求是否有主体       | 否   |
  | :------------------- | ---- |
  | 成功的响应是否有主体 | 是   |

- POST

  | 请求是否有主体       | 是   |
  | :------------------- | ---- |
  | 成功的响应是否有主体 | 是   |

- PATCH

  | Request has body             | Yes  |
  | :--------------------------- | ---- |
  | Successful response has body | No   |

- PUT

  | Request has body             | Yes  |
  | :--------------------------- | ---- |
  | Successful response has body | No   |

- DELETE

  | 请求是否有主体       | 可以有 |
  | :------------------- | ------ |
  | 成功的返回是否有主体 | 可以有 |

## 关于api的设计

- 应当遵守上述HTTP method的具体限定
- url应该为名词，比如/api/followers/
- url一般为复数，即加s，例子如上
- 一般情况下，GET查询参数位于url内，POST参数位于body即api中的Request，具体例子参见用户api-part2，其他http方法参照上面的表格

## 其他

- 班级表需要加一个是否需要组队
- **广播表需要加限定班级，不然班级广播好像没啥用**
- 展示课程表时，需要sec_id和course_id去找教学表，再通过找到的teacher_id去寻找老师的名字