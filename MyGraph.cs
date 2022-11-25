using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Katniss
{
    public class MyGraph : StudyBase
    {
        protected override void OnLog()
        {
            var graph = new Graph<string>();
            graph.Add("경기도");
            graph.Add("강원도");
            graph.Add("충청도");
            graph.Add("경상도");
            graph.Add("전라도");
            graph.Add("제주도");

            graph.SetEdge("경기도", "강원도", 7, 5);
            graph.SetEdge("경기도", "충청도", 5, true);

            graph.SetEdge("강원도", "충청도", 9, false);
            graph.SetEdge("강원도", "경상도", 13, true);

            graph.SetEdge("충청도", "경상도", 8, true);
            graph.SetEdge("충청도", "전라도", 7, true);

            graph.SetEdge("전라도", "경상도", 6, true);

            graph.SetEdge("경상도", "제주도", 14, false);
            graph.SetEdge("제주도", "경기도", 27, false);

            // 경상도 to 충청도 short (8) 경상도 > 충청도(8)
            // 경상도 to 충청도 long (57) 경상도 > 제주도(14) > 경기도(27) > 강원도(7) > 충청도(9)
            _ShortLongLog(graph.SearchAll("경상도", "충청도", Graph<string>.SearchPolicy.Visit));

            // 전라도 to 제주도 short (20) 전라도 > 경상도(6) > 제주도(14)
            // 전라도 to 제주도 long (46) 전라도 > 충청도(7) > 경기도(5) > 강원도(7) > 경상도(13) > 제주도(14)
            _ShortLongLog(graph.SearchAll("전라도", "제주도", Graph<string>.SearchPolicy.Visit));

            // 경기도 to 경상도 short (13) 경기도 > 충청도(5) > 경상도(8)
            // 경기도 to 경상도 long (48) 경기도 > 강원도(7) > 충청도(9) > 경기도(5) > 충청도(5) > 전라도(7) > 충청도(7) > 경상도(8)
            _ShortLongLog(graph.SearchAll("경기도", "경상도", Graph<string>.SearchPolicy.Pass));

            //강원도 to 전라도 short (16) 강원도 > 충청도(9) > 전라도(7)
            //강원도 to 전라도 long (108) 강원도 > 경기도(5) > 강원도(7) > 충청도(9) > 경상도(8) > 강원도(13) > 경상도(13) > 제주도(14) > 경기도(27) > 충청도(5) > 전라도(7)
            _ShortLongLog(graph.SearchAll("강원도", "전라도", Graph<string>.SearchPolicy.Pass));

            // 충청도 to 제주도 short (22) 충청도 > 경상도(8) > 제주도(14)
            // 충청도 to 제주도 long (96) 충청도 > 경기도(5) > 강원도(7) > 경기도(5) > 충청도(5) > 경상도(8) > 강원도(13) > 경상도(13) > 전라도(6) > 충청도(7) > 전라도(7) > 경상도(6) > 제주도(14)
            _ShortLongLog(graph.SearchAll("충청도", "제주도", Graph<string>.SearchPolicy.Pass));


            void _ShortLongLog<T>(List<GraphPath<T>> _paths)
            {
                if (_paths == null || _paths.Count < 1)
                    return;

                var _sPath = _paths[0];
                var _lPath = _sPath;

                var _sWeight = _sPath.GetTotalWeight();
                var _lWeight = _sWeight;

                for (int index = 1; index < _paths.Count; ++index)
                {
                    var _path = _paths[index];
                    var _weight = _path.GetTotalWeight();

                    if (_weight < _sWeight)
                    {
                        _sPath = _path;
                        _sWeight = _weight;
                    }
                    else if (_lWeight < _weight)
                    {
                        _lPath = _path;
                        _lWeight = _weight;
                    }
                }

                _PathLog($"{_sPath.Start.ThisVertex} to {_sPath.End.ThisVertex} short ", _sPath);
                _PathLog($"{_lPath.Start.ThisVertex} to {_lPath.End.ThisVertex} long ", _lPath);
            }

            void _PathLog<T>(string tag, GraphPath<T> _path)
            {
                if (_path.IsNoWay)
                {
                    Log($"{tag}(0) {_path.Start.ThisVertex} // {_path.End.ThisVertex}");
                    return;
                }

                var sb = new StringBuilder();
                sb.Append(_path.Start.ThisVertex);
                var _curNode = _path.Start;
                int _total = 0;
                for (int index = 1; index < _path.Count; ++index)
                {
                    _curNode.TryGetValue(_path.Vertexs[index], out var _edge);
                    _curNode = _edge.Node;
                    _total += _edge.Weight;

                    sb.Append($" > {_curNode.ThisVertex}({_edge.Weight})");
                }

                Log($"{tag}({_total}) {sb}");
            }
        }
    }
}